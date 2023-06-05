import {BaseService} from "@/services/base/BaseService";
import type {IBusiness} from "@/dto/shop/IBusiness";
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";

export interface IGetBusinessQueryParams {
    settlementId: string;
    businessCategoryId?: string;
}


export default class ShopsService extends BaseService {
    constructor() {
        super('/public/shops/');
    }


    async getBusinesses(jwtData: IJWTResponse, params: IGetBusinessQueryParams): Promise<IBusiness[] | undefined> {
        try {

            const response = await this.axios.get<IBusiness[]>('',
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    },
                    params: {
                        settlementId: params.settlementId,
                        businessCategoryId: params.businessCategoryId,
                    }
                }
            );

//            console.log('register response', response);
            if (response.status === 200) {
                return response.data;
            }
            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }


    async getBusiness(jwtData: IJWTResponse, businessId: string): Promise<IBusiness | undefined> {
        try {
            const response = await this.axios.get<IBusiness>(`/${businessId}`,
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

    async getBusinessInfo(jwtData: IJWTResponse, businessId: string): Promise<IBusiness | undefined> {
        try {
            const response = await this.axios.get<IBusiness>(`/${businessId}/info`,
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