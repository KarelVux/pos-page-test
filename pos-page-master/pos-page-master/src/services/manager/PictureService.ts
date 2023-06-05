import {BaseService} from "@/services/base/BaseService";
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";
import type {IManagerPicture} from "@/dto/manager/IManagerPicture";

export class PictureService extends BaseService {
    constructor() {
        super('/manager/pictures');
    }

    async postBusinessPicture(jwtData: IJWTResponse, businessId: string, fileData: File): Promise<IManagerPicture | undefined> {
        try {

            const formData = new FormData();
            formData.append('file', fileData);

            const response = await this.axios.post<IManagerPicture>(`/business/${businessId}`, formData,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            if (response.status === 201) {
                console.log('Business image uploaded successfully.');
                return response.data;
            } else {
                console.log('Failed to upload image.');
            }

            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }

    async postProductPicture(jwtData: IJWTResponse, productId: string, fileData: File): Promise<IManagerPicture | undefined> {
        try {

            const formData = new FormData();
            formData.append('file', fileData);

            const response = await this.axios.post<IManagerPicture>(`/product/${productId}`, formData,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            if (response.status === 201) {
                console.log('Product image uploaded successfully.');
                return response.data;
            } else {
                console.log('Failed to upload image.');
            }

            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }

    async deletePicture(jwtData: IJWTResponse, pictureId: string): Promise<number | undefined> {
        try {
            const response = await this.axios.delete(`/${pictureId}`,
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

}