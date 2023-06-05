<template>
    <div class="col-md-7 col-lg-8">
        <h4 class="mb-3">Search</h4>
        <div>
            <div class="row g-3">
                <div class="col-md-3">
                    <label for="zip" class="form-label">Business Name</label>
                    <input type="text" class="form-control" id="zip" placeholder=""
                           v-model="inputValues.businessSearchName" required>
                </div>
                <div class="col-md-5">
                    <label for="country" class="form-label">Settlement</label>
                    <select class="form-select" id="country" v-model="inputValues.settlementId" required>
                        <option value="">Choose...</option>
                        <option v-for="settlementItem in settlements" :key="settlementItem.id"
                                :value="settlementItem.id">{{ settlementItem.name }}
                        </option>
                    </select>
                </div>
                <div class="col-md-4">
                    <label for="state" class="form-label">Business Category</label>
                    <select class="form-select" id="businessCategory" v-model="inputValues.businessCategoryId">
                        <option value="">Choose...</option>
                        <option v-for="businessCategoryItem in businessCategories" :key="businessCategoryItem.id"
                                :value="businessCategoryItem.id">{{ businessCategoryItem.title }}
                        </option>
                    </select>
                </div>
            </div>
            <br>
            <button class="w-100 btn btn-primary btn-lg" @click="performSearch">Search</button>
        </div>
    </div>
    <br>

    <div class="col-lg-9">
        <div class="row justify-content-center mb-3">
            <div class="col-md-12">
                <div class="card shadow-0 border rounded-3">
                    <div class="card-body" v-for="businessData in businesses" :key="businessData.id">
                        <div class="row g-0">
                            <div class="col-xl-4 col-md-4 d-flex justify-content-center">
                                <div class="bg-image hover-zoom ripple rounded ripple-surface me-md-3 mb-3 mb-md-0">
                                    <img v-if="!businessData.picturePath || businessData.picturePath.length <= 0"
                                         src="https://images.unsplash.com/photo-1528698827591-e19ccd7bc23d?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1176&q=80"
                                         class="img-fluid" alt=""/>
                                    <img v-else
                                         :src="businessData.picturePath"
                                         class="img-fluid" alt=""/>
                                </div>
                            </div>
                            <div class="col-xl-8 col-md-8 col-sm-8">
                                <router-link :to="{ name: 'businessDetails', params: { id: businessData.id } }">
                                    <h5>{{ businessData.name }}</h5>
                                </router-link>


                                <div>
                                    <span class="badge bg-danger">{{ businessData.businessCategory.title }}</span>

                                </div>
                                <div>
                                    <p>Address: {{ businessData.address }}</p>
                                </div>
                                <p class="text mb-4 mb-md-0">
                                    {{ businessData.description }}
                                </p>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>
</template>

<script setup lang="ts">
import {useIdentityStore} from "@/stores/identityStore";
import type IBusinessSearch from "@/dto/shop/businessView/IBusinessSearch";
import type {IBusinessCategory} from "@/dto/shop/IBusinessCategory";
import type {IBusiness} from "@/dto/shop/IBusiness";
import ShopsService from "@/services/shop/ShopsService";
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";
import {redirectUserIfIdentityTokenIsNull} from "@/helpers/UserReidrecter";
import {onBeforeMount, onMounted, ref, watch} from "vue";
import {SettlementService} from "../../services/manager/SettlementService";
import {BusinessCategoriesService} from "../../services/manager/BusinessCategoriesService";
import type {IManagerSettlement} from "../../dto/manager/IManagerSettlement";


const identitySore = useIdentityStore();
const settlementService = new SettlementService();
const businessCategoriesService = new BusinessCategoriesService();
const shopsService = new ShopsService();

const inputValues = ref<IBusinessSearch>({
    businessCategoryId: "",
    businessSearchName: "",
    settlementId: ""
})

const businesses = ref<IBusiness[]>()
const settlements = ref<IManagerSettlement[]>()
const businessCategories = ref<IBusinessCategory[]>()
/*
const managerBusinessService = new ManagerBusinessService();
onBeforeMount(async () => {
    let identity = identitySore.authenticationJwt;

    if (identity){
        var value = await managerBusinessService.getAll(identity)
        console.log("USer businesses", value)

    }

});
*/

watch(() => identitySore.authenticationJwt, async () => {
    // do something when the data changes
    let identity = identitySore.authenticationJwt;

    settlements.value = (await settlementService.getAll(identity as IJWTResponse))
    businessCategories.value = (await businessCategoriesService.getAll(identity as IJWTResponse))

});

const performSearch = async () => {
    var responseBusinesses = await shopsService.getBusinesses(identitySore.authenticationJwt as IJWTResponse,
        {
            settlementId: inputValues.value.settlementId,
            businessCategoryId: inputValues.value.businessCategoryId
        })


    if (responseBusinesses) {
        if (responseBusinesses!.length > 0 &&
            inputValues.value.businessSearchName != null ||
            inputValues.value.businessSearchName!.trim().length > 0) {

            businesses.value = responseBusinesses.filter(x => x.name.toLowerCase().includes(inputValues.value.businessSearchName.toLowerCase().trim()))
        } else {
            businesses.value = responseBusinesses;

        }

    } else {
        businesses.value = []
    }
}

onBeforeMount(async () => {
    await redirectUserIfIdentityTokenIsNull();
});

onMounted(async () => {
    console.log("Open business details")
    let identity = identitySore.authenticationJwt;


    if (identity === undefined) {
        console.log("jwt is null")
        return;
    }
    settlements.value = (await settlementService.getAll(identity))
    businessCategories.value = (await businessCategoriesService.getAll(identity))
})
</script>