import {BaseEntityService} from '../base/BaseEntityService';
import type {IManagerBusiness} from "@/dto/manager/IManagerBusiness";

export class ManagerBusinessService extends BaseEntityService<IManagerBusiness> {
    constructor() {
        super('/manager/businesses');
    }
}