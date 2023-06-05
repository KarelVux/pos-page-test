import type {IBaseEntity} from "@/dto/management/IBaseEntity";
import type {IBusinessCategory} from "@/dto/shop/IBusinessCategory";
import type {IProduct} from "@/dto/shop/IProduct";
import type {IManagerSettlement} from "@/dto/manager/IManagerSettlement";

export interface IBusiness extends IBaseEntity {
    name: string,
    description: string,
    rating: number,
    longitude: number,
    latitude: number,
    address: string,
    phoneNumber: string,
    email: string,
    businessCategoryId: string,
    businessCategory: IBusinessCategory,
    picturePath?: string,
    settlementId: string,
    settlement: IManagerSettlement,
    products: IProduct[],
}