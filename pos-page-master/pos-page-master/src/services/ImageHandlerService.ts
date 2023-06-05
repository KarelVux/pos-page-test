import {BaseService} from "@/services/base/BaseService";
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";


export default class ImageHandlerService extends BaseService {
    constructor() {
        super('/images');
    }


    async uploadFile(jwtData: IJWTResponse, fileData: File): Promise<undefined> {
        try {

            const formData = new FormData();
            formData.append('file', fileData);

            const response = await this.axios.post('/upload', formData,
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            if (response.status === 200) {
                console.log('Image uploaded successfully.');
            } else {
                console.log('Failed to upload image.');
            }

            return undefined;
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return undefined;
        }
    }

    async getAllUploadedThumbnails(jwtData: IJWTResponse): Promise<string []> {
        try {


            const response = await this.axios.get<string[]>('thumbnails',
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            if (response.status === 200) {
                console.log('Image received');
                return response.data;
            } else {
                console.log('Failed to recieve image.');
            }

            return [];
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return [];
        }
    }


    /*
    async deleteThumbnails(jwtData: IJWTResponse): Promise<string []> {
        try {


            const response = await this.axios.delete<string[]>('thumbnails',
                {
                    headers: {
                        'Authorization': 'Bearer ' + jwtData.jwt
                    }
                }
            );

            if (response.status === 200) {
                console.log('Image deleted successfully.');
                return response.data;
            } else {
                console.log('Failed to deleted image.');
            }

            return [];
        } catch (e) {
            console.log('error: ', (e as Error).message);
            return [];
        }
    }
*/
}