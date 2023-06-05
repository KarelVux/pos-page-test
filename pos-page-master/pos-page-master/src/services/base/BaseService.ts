import type {AxiosInstance, AxiosRequestConfig, AxiosResponse} from 'axios';
import Axios, {AxiosError} from 'axios';
import {IdentityService} from "@/services/identity/IdentityService";
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";
import {useIdentityStore} from "@/stores/identityStore";
import type {IMessage} from "@/dto/shared/IMessage";
import {useMessageStore} from "@/stores/messageStore";
import {MessagePopupTypeEnum} from "@/components/shared/MessagePopupTypeEnum";

export abstract class BaseService {
    private static hostBaseURL = import.meta.env.VITE_BACKEND_URL;
    protected axios: AxiosInstance;


    constructor(baseUrl: string) {

        this.axios = Axios.create(
            {
                baseURL: BaseService.hostBaseURL + baseUrl,
                headers: {
                    common: {
                        'Content-Type': 'application/json'
                    }
                }
            }
        );

        this.axios.interceptors.request.use(request => {
            console.log('Starting Request', JSON.stringify(request, null, 2));
            return request;
        });

        const identityStore = useIdentityStore();

        this.axios.interceptors.response.use(
            (response: AxiosResponse) => {
                console.log('Received response', JSON.stringify(response, null, 2));

                return response;
            },
            async (error: AxiosError) => {
                if (error.response?.status === 401) {
                    console.error("Refreshing JWT");

                    const identityService = new IdentityService();
                    const refreshedJwt = await identityService.refreshToken(identityStore.$state.authenticationJwt as IJWTResponse);
                    if (refreshedJwt) {

                        console.log("JWT was successfully  refreshed")
                        identityStore.$state.authenticationJwt = refreshedJwt;

                        const config = error.config as AxiosRequestConfig | undefined;
                        if (config) {
                            config.headers!.Authorization = 'Bearer ' + refreshedJwt.jwt;
                            console.log("Sending previously failed request")

                            const newResponse = this.axios.request(config);
                            console.log(newResponse)
                            return newResponse;
                        }

                        /*
                                                error.config!.headers.Authorization = 'Bearer ' + refreshedJwt.jwt;

                                                console.log("Sending previously failed request")
                                                return this.axios.request(error.config)

                                                */
                    }


                    return Promise.reject(error);
                }

                const messageStore = useMessageStore();


                if (error.response) {
                    const message: IMessage = {message: error.message, status: error.response.statusText, type: MessagePopupTypeEnum.Error}


                    console.log("error.response",error.response)

                    message.status = error.response.status.toString() + " "+ error.response.statusText
                    message.message  = JSON.stringify(error.response.data)
                    messageStore.addMessage(message)


                    console.log(messageStore.getAllMessages())
                    console.log(messageStore.getAllMessages())

                    /*

                    for (const key in parsedJson as any) {
                        if (key.endsWith("errors") || key.endsWith("error")) {
                            message.message = (parsedJson as any)[key];
                        }
                    }


                    console.log("here is message data", message)
                    if (error.response.data) {


                      //   message.message = JSON.stringify(error.response.data)

                        //
                        //   const responseDate: IErrorData = (error.response.data) as IErrorData
//
                        //   console.log("Initial response",responseDate)
                        //   message.message = error.response.data as string

                        //   if (responseDate) {
                        //       message.status += ": " +responseDate.status.toString()


                        //       if (responseDate.errors) {
                        //           message.message = ": " + JSON.stringify(responseDate.errors)

                        //           console.log("response errprs", responseDate.errors)
                        //       }

                        //       if (responseDate.error) {
                        //           message.message += ": " + responseDate.error
                        //           console.log("response error", responseDate.error)

                        //       }

                        //       if (responseDate.message) {
                        //           message.message += ": " + responseDate.message;
                        //           console.log("response message", responseDate.message)

                        //       }
                        //   } else {
                        //       console.log("Error response",error.response.data)
                        //       message.message = error.response.data as string
                        //   }

                    }
*/
                }


            }
        )
    }
}


