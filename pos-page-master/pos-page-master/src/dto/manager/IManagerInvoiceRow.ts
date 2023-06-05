import type {IBaseEntity} from "@/dto/management/IBaseEntity";

export interface IManagerInvoiceRow extends IBaseEntity {
    finalProductPrice: number
    productUnitCount: number
    productPricePerUnit: number
    taxPercent: number
    taxAmountFromPercent : number
    currency: string
    comment: string
    productId: string
    productName: string
    invoiceId: string
}