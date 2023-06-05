import type {IBaseEntity} from "@/dto/management/IBaseEntity";

export interface IManagerBusinessPicture extends IBaseEntity {
    pictureId: string,
    businessId: string,
}