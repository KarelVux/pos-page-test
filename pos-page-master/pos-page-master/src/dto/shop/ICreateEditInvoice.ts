import type IInvoiceCreateEditProduct from "@/dto/shop/IInvoiceCreateEditProduct";
import type {IBaseEntity} from "@/dto/management/IBaseEntity";

export  interface ICreateEditInvoice extends  IBaseEntity{
    businessId?: string;
    InvoiceCreateEditProducts: IInvoiceCreateEditProduct[];
}