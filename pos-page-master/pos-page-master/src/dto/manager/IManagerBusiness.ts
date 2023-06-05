import type {IBaseEntity} from "@/dto/management/IBaseEntity";
import type {IManagerBusinessCategory} from "@/dto/manager/IManagerBusinessCategory";
import type {IManagerSettlement} from "@/dto/manager/IManagerSettlement";
import type {IManagerProduct} from "@/dto/manager/IManagerProduct";

export interface IManagerBusiness extends IBaseEntity {
    name: string
    description: string
    picturePath?: string
    rating: number
    longitude: number
    latitude: number
    address: string
    phoneNumber: string
    email: string
    businessCategoryId: string
    businessCategory: IManagerBusinessCategory
    settlementId: string
    settlement: IManagerSettlement
    products: IManagerProduct[]
}