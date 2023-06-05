import {BaseService} from "@/services/base/BaseService";
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";
import type {IOrderFeedback} from "@/dto/shop/IOrderFeedback";

export interface IGetBusinessQueryParams {
    settlementId: string;
    businessCategoryId?: string;
}


export default class OrderFeedbackService extends BaseService {
    constructor() {
        super('/public/orderFeedbacks');
    }


    async getOrderFeedback(jwtData: IJWTResponse, orderFeedbackId: string): Promise<IOrderFeedback | undefined> {
        try {
            const response = await this.axios.get<IOrderFeedback>(`/${orderFeedbackId}`,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            console.log('register response', response);
            if (response.status === 200) {
                return response.data;
            }
            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }
    async getOrderFeedbackViaOrderId(jwtData: IJWTResponse, orderId: string): Promise<IOrderFeedback | undefined> {
        try {
            const response = await this.axios.get<IOrderFeedback>(`/${orderId}/orderId`,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            console.log('register response', response);
            if (response.status === 200) {
                return response.data;
            }
            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }

    async createOrderFeedback(jwtData: IJWTResponse, entity: IOrderFeedback): Promise<IOrderFeedback | undefined> {
        try {
            const response = await this.axios.post<IOrderFeedback>('', entity,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            console.log('response', response);
            if (response.status === 201) {
                return response.data;
            }

            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message, e);
            return undefined;
        }
    }

}