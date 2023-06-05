import type {IBaseEntity} from "@/dto/management/IBaseEntity";
import type {OrderAcceptanceStatusEnum} from "@/dto/enums/OrderAcceptanceStatusEnum";

export interface IOrder extends IBaseEntity {
    startTime: Date
    givenToClientTime: Date
    orderAcceptanceStatus: OrderAcceptanceStatusEnum
    customerComment: string
    invoiceId: string
}