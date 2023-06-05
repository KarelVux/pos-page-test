import {BaseService} from "@/services/base/BaseService";
import type {IManagerProductPicture} from "@/dto/manager/IManagerProductPicture";

export class ProductPictureService extends BaseService {
    constructor() {
        super('/manager/productPicture');
    }

    async getProductPicturePicture(jwtData: any, businessId: string): Promise<IManagerProductPicture | undefined> {
        try {
            const response = await this.axios.get<IManagerProductPicture>(`/product/${businessId}`,
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