import {BaseService} from "@/services/base/BaseService";
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";
import type {ICreateEditInvoice} from "@/dto/shop/ICreateEditInvoice";
import type {IInvoice} from "@/dto/shop/IInvoice";
import type {IAcceptInvoice} from "@/dto/shop/IAcceptInvoice";

export interface IGetBusinessQueryParams {
    settlementId: string;
    businessCategoryId?: string;
}


export default class InvoicesService extends BaseService {
    constructor() {
        super('/public/invoices/');
    }


    async createInvoice(jwtData: IJWTResponse, createEditInvoice: ICreateEditInvoice): Promise<ICreateEditInvoice | undefined> {
        try {
            const response = await this.axios.post<ICreateEditInvoice>(``, createEditInvoice,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            if (response.status === 201) {
                return response.data;
            }
            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }

    async getInvoice(jwtData: IJWTResponse, invoiceId: string): Promise<IInvoice | undefined> {
        try {
            const response = await this.axios.get<IInvoice>(`${invoiceId}`,
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


    async acceptInvoice(jwtData: IJWTResponse, invoiceId: string, acceptance: IAcceptInvoice): Promise<number | undefined> {
        try {
            const response = await this.axios.patch(`/${invoiceId}/acceptance`,
                acceptance,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    },
                }
            );

            console.log('register response', response);
            if (response.status === 204) {
                return response.status;
            }
            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }


    async deleteInvoice(jwtData: IJWTResponse, invoiceId: string): Promise<number | undefined> {
        try {
            const response = await this.axios.delete(`/${invoiceId}`,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    },
                }
            );

            console.log('register response', response);
            if (response.status === 204) {
                return response.status;
            }
            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }


    async getAllUserInvoices(jwtData: IJWTResponse): Promise<IInvoice [] | undefined> {
        try {
            const response = await this.axios.get(``,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    },
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