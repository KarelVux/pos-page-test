import type {IBaseEntity} from "@/dto/management/IBaseEntity";

export interface IInvoiceRow extends IBaseEntity {
    productName: string
    finalProductPrice: number
    productUnitCount: bigint
    productPricePerUnit: number
    taxPercent: number
    taxAmountFromPercent: number
    productId: string
    currency: string
    comment: string,
    invoiceId: string
}