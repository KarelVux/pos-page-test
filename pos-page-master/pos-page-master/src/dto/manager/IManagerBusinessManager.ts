import type {IBaseEntity} from "@/dto/management/IBaseEntity";

export interface IManagerBusinessManager extends IBaseEntity {
    appUserId: string,
    businessId: string,
}