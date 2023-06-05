
import type {IOrder} from "@/dto/shop/IOrder";

export interface IInvoiceOrder {
    invoiceId: string
    businessId: string
    order: IOrder

}