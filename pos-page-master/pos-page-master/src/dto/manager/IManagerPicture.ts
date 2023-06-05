import type {IBaseEntity} from "@/dto/management/IBaseEntity";

export interface IManagerPicture extends IBaseEntity {
    title: string
    description: string
    path: string
}