<template>
    <!-- Start modal-->
    <section>
        <div class="d-flex">

            <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal"
                    :data-bs-target="'#' + uniqueId">
                <span v-if="props.create">
                Add new business
                </span>
                <span v-else>
                Edit
                </span>

            </button>
            <!-- Modal -->
            <div class="modal fade" :id="uniqueId" data-bs-backdrop="static" data-bs-keyboard="false"
                 tabindex="-1" :aria-labelledby="uniqueId + 'Label'" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h1 class="modal-title fs-5" :id="uniqueId + 'Label'">
                                <span v-if="props.create">
                                    Add new business
                                </span>
                                <span v-else>
                                    Edit
                                </span>

                            </h1>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"
                                    aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-md-6 mb-3">
                                    <label>Business Name</label>
                                    <input type="text" class="form-control"
                                           v-model="displayData.name">

                                </div>

                                <div class="col-md-6 mb-3">
                                    <label>Address</label>
                                    <input type="text" class="form-control"
                                           v-model="displayData.address">

                                </div>

                                <div class="col-md-6 mb-3">
                                    <label>Phone number</label>
                                    <input type="text" class="form-control"
                                           v-model="displayData.phoneNumber">
                                </div>


                                <div class="col-md-6 mb-3">
                                    <label>Email</label>
                                    <input type="email" class="form-control"
                                           v-model="displayData.email">
                                </div>

                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Business Category</label>
                                    <select class="form-select"
                                            v-model="displayData.businessCategoryId">
                                        <option value="-1" selected disabled>Please select value</option>
                                        <option v-for="businessCategoryItem in businessCategories"
                                                :key="businessCategoryItem.id"
                                                :value="businessCategoryItem.id">
                                            {{ businessCategoryItem.title }}
                                        </option>
                                    </select>
                                </div>
                                <div class="col-md-6 mb-3">
                                    <label class="form-label">Settlement</label>
                                    <select class="form-select"
                                            v-model="displayData.settlementId">
                                        <option value="-1" selected disabled>Please select value</option>
                                        <option
                                            v-for="settlementItem in settlements"
                                            :key="settlementItem.id"
                                            :value="settlementItem.id">
                                            {{ settlementItem.name }}
                                        </option>
                                    </select>
                                </div>
                            </div>
                            <div class="mb-3">
                                <label class="form-label">Description</label>
                                <input type="text" class="form-control"
                                       v-model="displayData.description">
                            </div>

                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close
                            </button>
                            <button type="button" class="btn btn-primary" v-on:click="onSubmit">
                                <span v-if="props.create">
                                    Add new business
                                </span>
                                <span v-else>
                                    Save Updates
                                </span>
                            </button>
                            <p :id="uniqueHiderId" data-bs-dismiss="modal" style="visibility: hidden"></p>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </section>
    <!-- End modal-->
</template>

<script lang="ts" setup>


import {onBeforeMount, ref, watch} from "vue";
import type {IManagerBusiness} from "@/dto/manager/IManagerBusiness";
import {ManagerBusinessService} from "@/services/manager/ManagerBusinessService";
import {useIdentityStore} from "@/stores/identityStore";
import {generateRandomString} from "@/helpers/Randomiser";
import type {IManagerSettlement} from "@/dto/manager/IManagerSettlement";
import type {IManagerBusinessCategory} from "@/dto/manager/IManagerBusinessCategory";
import {SettlementService} from "@/services/manager/SettlementService";
import {BusinessCategoriesService} from "@/services/manager/BusinessCategoriesService";

const managerBusinessService = new ManagerBusinessService();
const identitySore = useIdentityStore();
const settlementService = new SettlementService();
const businessCategoriesService = new BusinessCategoriesService();


interface IProps {
    businessData: IManagerBusiness,
    settlements: IManagerSettlement[],
    businessCategories: IManagerBusinessCategory[],
    create: boolean
}

const uniqueId = ref<string>('s' + generateRandomString())
const uniqueHiderId = ref<string>('s' + generateRandomString())


// Define the props and emits
const props = defineProps<IProps>();
const emits = defineEmits(['update']);

const settlements = ref<IManagerSettlement[]>(props.settlements)
const businessCategories = ref<IManagerBusinessCategory[]>(props.businessCategories)

// Create a localData ref to hold the updated values
const originalData = ref<IManagerBusiness>(props.businessData)
const displayData = ref<IManagerBusiness>({
    address: "",
    businessCategoryId: "",
    description: "",
    email: "",
    name: "",
    phoneNumber: "",
    settlementId: ""
} as IManagerBusiness)

onBeforeMount(async () => {
    await sendUserBusinessViewRequests();

    if (!props.create) {
        originalToDisplay();

    }
})

const originalToDisplay = () => {
    displayData.value.id = originalData.value.id
    displayData.value.address = originalData.value.address
    displayData.value.businessCategoryId = originalData.value.businessCategoryId
    displayData.value.description = originalData.value.description
    displayData.value.email = originalData.value.email
    displayData.value.name = originalData.value.name
    displayData.value.phoneNumber = originalData.value.phoneNumber
    displayData.value.settlementId = originalData.value.settlementId
}


const sendUserBusinessViewRequests = async () => {
    let identity = identitySore.authenticationJwt

    if (identity) {
        settlements.value = (await settlementService.getAll(identity))!
        businessCategories.value = (await businessCategoriesService.getAll(identity))!
    }
}


watch(() => props.settlements, (newSettlement) => {
    settlements.value = newSettlement;
});

watch(() => props.businessCategories, (newBusinessCategory) => {
    businessCategories.value = newBusinessCategory;
});

const onSubmit = async (event: MouseEvent) => {
    event.preventDefault();

    let identity = identitySore.authenticationJwt;

    if (identity === undefined) {
        console.log("jwt is null")
        return;
    }
    let business: IManagerBusiness | undefined;
    if (props.create) {
        business = await managerBusinessService.create(identity, displayData.value)
        if (business) {
            console.log("Business creation was successful")
        } else {
            console.error("Unable to create business")
            return
        }
    } else {

        // mapDisplayToOriginal();


        let [, status] = await managerBusinessService.update(identity, displayData.value.id!, displayData.value)
        if (status) {
            console.log("Business Edit was successful")
        } else {
            console.error("Error occurred when editing business")
            return
        }
    }

    emits('update');
    let hider = document.getElementById(uniqueHiderId.value);
    hider!.click()

}
</script>