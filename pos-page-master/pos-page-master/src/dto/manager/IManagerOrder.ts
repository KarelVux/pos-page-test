import type {IBaseEntity} from "@/dto/management/IBaseEntity";
import type {IManagerInvoice} from "@/dto/manager/IManagerInvoice";
import type {IManagerOrderFeedback} from "@/dto/manager/IManagerOrderFeedback";
import type {OrderAcceptanceStatusEnum} from "@/dto/enums/OrderAcceptanceStatusEnum";

export interface IManagerOrder extends IBaseEntity {
    startTime: Date
    givenToClientTime: Date
    orderAcceptanceStatus: OrderAcceptanceStatusEnum
    customerComment: string
    invoiceId: string
    invoice: IManagerInvoice
    orderFeedback: IManagerOrderFeedback
}