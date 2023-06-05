import {BaseService} from "@/services/base/BaseService";
import type {IManagerBusinessPicture} from "@/dto/manager/IManagerBusinessPicture";

export class BusinessPictureService extends BaseService {
    constructor() {
        super('/manager/businessPictures');
    }

    async getBusinessPicture(jwtData: any, businessId: string): Promise<IManagerBusinessPicture | undefined> {
        try {
            const response = await this.axios.get<IManagerBusinessPicture>(`/business/${businessId}`,
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