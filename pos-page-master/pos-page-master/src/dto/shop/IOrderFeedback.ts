import type {IBaseEntity} from "@/dto/management/IBaseEntity";

export interface IOrderFeedback extends IBaseEntity {
    title: string
    description: string
    rating: number
    orderId: string
}