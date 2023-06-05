import type {IJWTResponse} from "@/dto/identity/IJWTResponse";
import {BaseService} from "@/services/base/BaseService";
import type {IManagerProductCategory} from "@/dto/manager/IManagerProductCategory";

export class ProductCategoryService extends BaseService {
    constructor() {
        super('/manager/productCategories');
    }


    async getAll(jwtData: IJWTResponse): Promise<IManagerProductCategory[] | undefined> {
        try {
            const response = await this.axios.get<IManagerProductCategory[]>('',
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

    async getById(jwtData: IJWTResponse, id: string): Promise<IManagerProductCategory | undefined> {
        try {
            const response = await this.axios.get<IManagerProductCategory>(`/${id}`,
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

    async create(jwtData: IJWTResponse, entity: IManagerProductCategory): Promise<IManagerProductCategory | undefined> {
        try {
            const response = await this.axios.post<IManagerProductCategory>('', entity,
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