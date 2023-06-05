import {BaseService} from "@/services/base/BaseService";
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";
import type {IManagerOrderFeedback} from "@/dto/manager/IManagerOrderFeedback";

export interface IGetBusinessQueryParams {
    settlementId: string;
    businessCategoryId?: string;
}


export default class OrderFeedbackService extends BaseService {
    constructor() {
        super('/manager/orderFeedbacks');
    }


    async getOrderFeedback(jwtData: IJWTResponse, orderFeedbackId: string): Promise<IManagerOrderFeedback | undefined> {
        try {
            const response = await this.axios.get<IManagerOrderFeedback>(`/${orderFeedbackId}`,
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

    async getOrderFeedbackViaOrderId(jwtData: IJWTResponse, orderId: string): Promise<IManagerOrderFeedback | undefined> {
        try {
            const response = await this.axios.get<IManagerOrderFeedback>(`/${orderId}/orderId`,
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
}