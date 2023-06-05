import type {IBaseEntity} from "@/dto/management/IBaseEntity";
import type {IManagerProductCategory} from "@/dto/manager/IManagerProductCategory";


export interface IManagerProduct extends IBaseEntity {
    name: string
    description: string
    picturePath?: string
    unitPrice: number
    unitDiscount: number
    unitCount: number
    taxPercent: number
    currency: string
    frozen: boolean
    productCategoryId: string
    productCategory: IManagerProductCategory
    businessId: string
}