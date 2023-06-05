import type {InvoiceAcceptanceStatusEnum} from "@/dto/enums/InvoiceAcceptanceStatusEnum";
import type {IBaseEntity} from "@/dto/management/IBaseEntity";
import type {OrderAcceptanceStatusEnum} from "@/dto/enums/OrderAcceptanceStatusEnum";

export interface IManagerInvoiceOrderLimitedEdit extends IBaseEntity {
    paymentCompleted: boolean,
    invoiceAcceptanceStatus: InvoiceAcceptanceStatusEnum,
    orderAcceptanceStatus: OrderAcceptanceStatusEnum
}