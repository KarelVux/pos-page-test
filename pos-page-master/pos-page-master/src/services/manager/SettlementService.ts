import type {IJWTResponse} from "@/dto/identity/IJWTResponse";
import {BaseService} from "@/services/base/BaseService";
import type {IManagerSettlement} from "@/dto/manager/IManagerSettlement";

export class SettlementService extends BaseService {
    constructor() {
        super('/manager/settlements');
    }


    async getAll(jwtData: IJWTResponse): Promise<IManagerSettlement[] | undefined> {
        try {
            const response = await this.axios.get<IManagerSettlement[]>('',
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            console.log('response', response);
            if (response.status === 200) {
                return response.data;
            }

            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message, e);
            return undefined;
        }
    }

    async getById(jwtData: IJWTResponse, id: string): Promise<IManagerSettlement | undefined> {
        try {
            const response = await this.axios.get<IManagerSettlement>(`/${id}`,
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

    async create(jwtData: IJWTResponse, entity: IManagerSettlement): Promise<IManagerSettlement | undefined> {
        try {
            const response = await this.axios.post<IManagerSettlement>('', entity,
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