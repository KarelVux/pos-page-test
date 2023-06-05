import type {IBaseEntity} from "@/dto/management/IBaseEntity";

export interface IManagerProductPicture extends IBaseEntity {
    pictureId: string
    productId: string
}