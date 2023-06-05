import type {IManagerBusinessCategory} from "@/dto/manager/IManagerBusinessCategory";
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";
import {BaseService} from "@/services/base/BaseService";

export class BusinessCategoriesService extends BaseService {
    constructor() {
        super('/manager/BusinessCategories');
    }


    async getAll(jwtData: IJWTResponse): Promise<IManagerBusinessCategory[] | undefined> {
        try {
            const response = await this.axios.get<IManagerBusinessCategory[]>('',
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

    async getById(jwtData: IJWTResponse, id: string): Promise<IManagerBusinessCategory | undefined> {
        try {
            const response = await this.axios.get<IManagerBusinessCategory>(`/${id}`,
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

    async create(jwtData: IJWTResponse, entity: IManagerBusinessCategory): Promise<IManagerBusinessCategory | undefined> {
        try {
            const response = await this.axios.post<IManagerBusinessCategory>('', entity,
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