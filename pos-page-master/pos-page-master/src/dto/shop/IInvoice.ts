import type {IBaseEntity} from "@/dto/management/IBaseEntity";
import type {IOrder} from "@/dto/shop/IOrder";
import type {IInvoiceRow} from "@/dto/shop/IInvoiceRow";
import type {InvoiceAcceptanceStatusEnum} from "@/dto/enums/InvoiceAcceptanceStatusEnum";

export interface IInvoice extends IBaseEntity {
    finalTotalPrice: number
    taxPercent: number
    taxAmount: number
    totalPriceWithoutTax: number
    paymentCompleted: boolean
    invoiceAcceptanceStatus: InvoiceAcceptanceStatusEnum,
    creationTime: Date
    businessId: string
    businessName?: string
    orderId: string
    order?: IOrder
    invoiceRows: IInvoiceRow []
}