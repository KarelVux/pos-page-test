import {BaseEntityService} from '../base/BaseEntityService';
import type {IManagerProduct} from "@/dto/manager/IManagerProduct";

export class ProductService extends BaseEntityService<IManagerProduct> {
    constructor() {
        super('/manager/product');
    }
}