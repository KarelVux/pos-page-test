import type {IBaseEntity} from "@/dto/management/IBaseEntity";
import type {IManagerOrder} from "@/dto/manager/IManagerOrder";
import type {IManagerInvoiceRow} from "@/dto/manager/IManagerInvoiceRow";
import type {InvoiceAcceptanceStatusEnum} from "@/dto/enums/InvoiceAcceptanceStatusEnum";

export interface IManagerInvoice extends IBaseEntity {
    finalTotalPrice: number,
    taxAmount: number,
    totalPriceWithoutTax: number,
    paymentCompleted: boolean,
    invoiceAcceptanceStatus: InvoiceAcceptanceStatusEnum,
    creationTime: Date
    appUserId: string
    userName?: string
    businessId: string
    orderId: string
    order: IManagerOrder
    invoiceRows?: IManagerInvoiceRow[]
}