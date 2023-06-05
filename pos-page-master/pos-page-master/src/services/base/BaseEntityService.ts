import {BaseService} from "./BaseService";
import type {IBaseEntity} from "@/dto/management/IBaseEntity";
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";

export abstract class BaseEntityService<TEntity extends IBaseEntity> extends BaseService {
    constructor(
        baseUrl: string,
    ) {
        super(baseUrl);
    }

    async getAll(jwtData: IJWTResponse): Promise<TEntity[] | undefined> {
        try {
            const response = await this.axios.get<TEntity[]>('',
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

    async getById(jwtData: IJWTResponse, id: string): Promise<TEntity | undefined> {
        try {
            const response = await this.axios.get<TEntity>(`/${id}`,
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

    async create(jwtData: IJWTResponse, entity: TEntity): Promise<TEntity | undefined> {
        try {
            const response = await this.axios.post<TEntity>('', entity,
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


    async update(jwtData: IJWTResponse, id: string, entity: TEntity): Promise<[TEntity | undefined, boolean]> {
        try {
            const response = await this.axios.put<TEntity>(`/${id}`, entity,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            console.log('response', response);
            if (response.status === 200 || response.status === 204) {
                return [response.data, true];
            }

            return [undefined, false];
        } catch (e) {
            console.log('error: ', (e as Error).message, e);
            return [undefined, false];
        }
    }


}