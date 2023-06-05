<template>
    <div class="row justify-content-center mb-3">
        <div class="col-md-12">
            <section v-if="userInManagerRole">

                <!-- Start modal-->
                <section>
                    <div class="d-flex">
                        <h1 class="pe-3">Owned businesses</h1>

                        <BusinessCreateEditModal :create="true"
                                                 @update="updateObjectData"
                                                 :businessData="{} as IManagerBusiness"
                                                 :businessCategories="businessCategories"
                                                 :settlements="settlements"
                        />

                        <BusinessCategoryCreateModal
                            @update="updateObjectData"
                        />
                        <SettlementCreateModal
                            @update="updateObjectData"
                        />

                    </div>
                </section>
                <!-- End modal-->

                <!-- Start business loading-->
                <div class="table-responsive" v-if="managerBusinessesData">
                    <table class="table table-striped table-sm">
                        <thead>
                        <tr>
                            <th>
                                Business ID
                            </th>
                            <th>
                                Business name
                            </th>
                            <th>
                                Address
                            </th>
                            <th>
                                Business description
                            </th>
                        </tr>
                        </thead>
                        <tbody>
                        <tr v-for="item  in managerBusinessesData" :key="item.id" class="a">
                            <td class="">
                                <RouterLink :to="{ name: 'userBusinessesManagement', params: { id: item.id } }">
                                    {{
                                        item.id
                                    }}
                                </RouterLink>
                            </td>
                            <td>
                                <RouterLink :to="{ name: 'userBusinessesManagement', params: { id: item.id } }">
                                    {{
                                        item.name
                                    }}
                                </RouterLink>
                            </td>
                            <td>
                                {{ item.address }}
                            </td>
                            <td>
                                {{ item.description }}
                            </td>

                        </tr>
                        </tbody>
                    </table>
                </div>
                <div v-else>
                    <LoadingData/>
                </div>
                <!-- End business loading-->
            </section>
            <section v-else>
                <div class="card text-center">

                    <div class="card-body">
                        <h5 class="card-title">
                            Do you want to add your own business?
                        </h5>
                        <div>
                            <button type="button" class="btn btn-primary mx-2" @click="router.go(-1)">Go back</button>
                            <button type="button" class="btn btn-primary mx-2" @click="addUserToTheRoleAndNavigate">
                                Right
                            </button>
                        </div>
                    </div>
                </div>
            </section>
        </div>
    </div>


</template>


<script lang="ts" setup>

import {useIdentityStore} from "@/stores/identityStore";
import {ManagerBusinessService} from "@/services/manager/ManagerBusinessService";
import {onBeforeMount, onMounted, ref, watch} from "vue";
import type {IManagerBusiness} from "@/dto/manager/IManagerBusiness";
import {useMessageStore} from "@/stores/messageStore";
import type {IBusinessCategory} from "@/dto/shop/IBusinessCategory";
import LoadingData from "@/components/shared/LoadingData.vue";
import BusinessCreateEditModal from "@/components/manager/BusinessCreateEditModal.vue";
import {findUserRoleFromJwt} from "@/helpers/jwtHelper";
import {useRouter} from "vue-router";
import {IdentityService} from "@/services/identity/IdentityService";
import {MessagePopupTypeEnum} from "@/components/shared/MessagePopupTypeEnum";
import {redirectUserIfIdentityTokenIsNull} from "@/helpers/UserReidrecter";
import BusinessCategoryCreateModal from "@/components/manager/BusinessCategoryCreateModal.vue";
import SettlementCreateModal from "@/components/manager/SettlementCreateModal.vue";
import {SettlementService} from "@/services/manager/SettlementService";
import {BusinessCategoriesService} from "@/services/manager/BusinessCategoriesService";
import type {IManagerSettlement} from "@/dto/manager/IManagerSettlement";

const managerBusinessService = new ManagerBusinessService();
const identitySore = useIdentityStore();
const messageStore = useMessageStore();
const settlementService = new SettlementService();
const identityService = new IdentityService();
const businessCategoriesService = new BusinessCategoriesService();
const router = useRouter()

const settlements = ref<IManagerSettlement[]>([])
const businessCategories = ref<IBusinessCategory[]>([])


const managerBusinessesData = ref<IManagerBusiness[]>([]);



const userInManagerRole = ref<boolean>(false)

onBeforeMount(async () => {
    await redirectUserIfIdentityTokenIsNull();

    let identity = identitySore.authenticationJwt;

    let userRole = findUserRoleFromJwt(identity!.jwt)
    console.log("User role ", userRole)

    if (userRole === "BusinessManager") {
        userInManagerRole.value = true
        await sendUserBusinessViewRequests();
    }

})


const sendUserBusinessViewRequests = async () => {
    let identity = identitySore.authenticationJwt

    if (identity) {
        let businesses = await managerBusinessService.getAll(identity)
        if (businesses) {
            managerBusinessesData.value = businesses;

            console.log(managerBusinessesData.value)
        } else {
            messageStore.addMessage({
                message: "Unable to initialize user businesses",
                status: "",
                type: MessagePopupTypeEnum.Error
            })
        }

        settlements.value = (await settlementService.getAll(identity))!
        businessCategories.value = (await businessCategoriesService.getAll(identity))!

    }
}

const addUserToTheRoleAndNavigate = async () => {
    let identity = identitySore.authenticationJwt

    if (identity) {
        let roleAdding = await identityService.addUserToBusinessManagerRole(identity)

        // Update JWT because user roles are added to it
        await identityService.refreshToken(identity)
        if (roleAdding) {
            await sendUserBusinessViewRequests();
            userInManagerRole.value = true

        } else {
            messageStore.addMessage({
                message: "Error occurred when initializing user access", status: "", type: MessagePopupTypeEnum.Error

            })
            return
        }
    }

}


onMounted(async () => {
    console.log("Open business details")
    let identity = identitySore.authenticationJwt;

    if (identity === undefined) {
        console.log("jwt is null")
        return;
    }
})

watch(() => [managerBusinessesData.value, settlements.value, businessCategories.value], async () => {
    // do something when the data changes
});

const updateObjectData = async () => {
    await sendUserBusinessViewRequests();
}
</script>