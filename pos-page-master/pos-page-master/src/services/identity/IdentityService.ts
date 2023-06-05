import {BaseService} from "@/services/base/BaseService";
import type {IRegisterData} from "@/dto/identity/IRegisterData";
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";
import type {ILoginData} from "@/dto/identity/ILoginData";
import {useIdentityStore} from "@/stores/identityStore";
import {findUserNameFromJwt} from "@/helpers/jwtHelper";

export class IdentityService extends BaseService {
    constructor() {
        super('/identity/account/');
    }

    identityStore = useIdentityStore();

    async register(data: IRegisterData): Promise<IJWTResponse | undefined> {
        try {
            const response = await this.axios.post<IJWTResponse>('register', data);

            console.log('register response', response);
            if (response.status === 200) {
                this.identityStore.$state.authenticationJwt = response.data;
                return response.data;
            }
            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }

    async login(data: ILoginData): Promise<IJWTResponse | undefined> {
        try {
            const response = await this.axios.post<IJWTResponse>('login', data);

            console.log('login response', response);
            if (response.status === 200) {
                this.identityStore.$state.authenticationJwt = response.data;
                const username = findUserNameFromJwt(this.identityStore.$state.authenticationJwt.jwt);
                if (username) {
                    this.identityStore.$state.userName = username;

                }
                return response.data;
            }

            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }

    async logout(data: IJWTResponse): Promise<true | undefined> {
        console.log(data);

        try {
            const response = await this.axios.post(
                'logout',
                data,
                {
                    headers: {
                        'Authorization': 'Bearer ' + data.jwt
                    }
                }
            );

            console.log('logout response', response);
            if (response.status === 200) {
                this.identityStore.$state.authenticationJwt = undefined;
                this.identityStore.$state.userName = undefined;

                return true;
            }
            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }

    async refreshToken(data: IJWTResponse): Promise<IJWTResponse | undefined> {
        try {
            const response = await this.axios.post<IJWTResponse>(
                'refreshtoken',
                data
            );

            console.log('refresh token response', response);
            if (response.status === 200) {
                this.identityStore.$state.authenticationJwt = response.data;
                return response.data;
            }
            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }

    async addUserToBusinessManagerRole(data: IJWTResponse): Promise<boolean | undefined> {
        try {
            const response = await this.axios.post('addUserToBusinessManagerRole',
                null
                , {
                    headers: {
                        'Authorization': 'Bearer ' + data.jwt
                    }
                });
            if (response.status === 200) {
                return true;
            }

            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }
}