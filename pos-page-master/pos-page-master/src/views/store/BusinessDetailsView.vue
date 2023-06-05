<template>
    <main class="">
        <div v-if="businessDetails" class="container mt-5">
            <BusinessIntroduction :businessDetails="businessDetails"/>
            <section>
                <div class="container">
                    <div class="col-lg-12 ">
                        <header
                            class="d-sm-flex align-items-center border-bottom mb-4 pb-3 d-flex flex-row justify-content-between">
                            <strong class="d-block py-2">{{ businessDetails.products.length }} Items found </strong>
                            You have selected {{ selectedProducts.length }} products
                            <!-- Button trigger modal -->
                            <button
                                v-if="selectedProducts.length > 0"
                                type="button"
                                class="btn btn-primary"
                                data-bs-toggle="modal"
                                data-bs-target="#exampleModal"
                                @click="createInvoice">
                                Create invoice 2
                            </button>

                            <!-- Modal -->
                            <div class="modal fade" id="exampleModal" tabindex="-1" aria-labelledby="exampleModalLabel"
                                 aria-hidden="true">
                                <div class="modal-dialog">
                                    <div class="modal-content">

                                        <div class="modal-body">
                                            <InvoiceDetailsCard :invoiceDataVal="invoiceData!">
                                                <button
                                                    @click="goBackAndEdit"
                                                    class="btn btn-warning btn-lg card-footer-btn justify-content-center text-uppercase-bold-sm hover-lift-light">
                                                    Go and Edit
                                                </button>
                                                <!--          <RouterLink :to="{name:'invoiceOrder', params: {id: route.params.id}}"-->
                                                <!--                      class="btn btn-dark btn-lg card-footer-btn justify-content-center text-uppercase-bold-sm hover-lift-light"-->
                                                <!--          >Edit-->
                                                <!--          </RouterLink>-->
                                                <button
                                                    @click="acceptInput"
                                                    class="btn btn-dark btn-lg card-footer-btn justify-content-center text-uppercase-bold-sm hover-lift-light">
                                                    Accept and Order
                                                </button>

                                                <div class="modal-footer">
                                                    <p id="closeModal" data-bs-dismiss="modal"
                                                       style="visibility: hidden"></p>
                                                </div>
                                            </InvoiceDetailsCard>
                                        </div>
                                    </div>
                                </div>
                            </div>


                            <div v-if="showCreationModal">

                            </div>

                        </header>

                        <div class="row justify-content-center mb-3" v-for="product in businessDetails.products"
                             :key="product.id">
                            <div class="col-md-12">
                                <div class="card shadow-0 border rounded-3">
                                    <div class="card-body">
                                        <div class="row g-0">
                                            <div class="col-xl-3 col-md-4 d-flex justify-content-center">
                                                <div
                                                    class="bg-image hover-zoom ripple rounded ripple-surface me-md-3 mb-3 mb-md-0">
                                                    <img v-if="!product.picturePath || product.picturePath.length <= 0"
                                                         src="https://images.unsplash.com/photo-1528698827591-e19ccd7bc23d?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1176&q=80"
                                                         class="img-fluid w-100" alt=""/>
                                                    <img v-else
                                                         :src="product.picturePath"
                                                         class="img-fluid w-100" alt=""/>

                                                    <a href="#!">
                                                        <div class="hover-overlay">
                                                            <div class="mask"
                                                                 style="background-color: rgba(253, 253, 253, 0.15);"></div>
                                                        </div>
                                                    </a>
                                                </div>
                                            </div>
                                            <div class="col-xl-7 col-md-6 col-sm-8">
                                                <h5>{{ product.name }}</h5>
                                                <div class="d-flex flex-row">

                                                    <div class="mb-3">
                                                        <span class="badge bg-info me-1">{{
                                                                product.productCategory.title
                                                            }}</span>
                                                    </div>


                                                </div>

                                                <p class="text mb-4 mb-md-0">
                                                    {{ product.description }}
                                                </p>
                                            </div>
                                            <div class="col-xl-2 col-md-2 col-sm-4">

                                                <div v-if="product.unitDiscount"
                                                     class="d-flex flex-row align-items-center mb-1">
                                                    <div v-if="product.userSelectedProductCount">
                                                        {{ product.userSelectedProductCount }}
                                                        X
                                                        {{ product.unitPrice + product.unitDiscount }}
                                                        {{ product.currency }}
                                                        <span class="text-danger">
                                                            <s>
                                                                {{ product.unitPrice }} {{ product.currency }}
                                                            </s>
                                                        </span>

                                                        =
                                                        {{
                                                            product.userSelectedProductCount * (product.unitPrice + product.unitDiscount)
                                                        }}
                                                        {{ product.currency }}

                                                        <span class="text-danger">
                                                            <s>
                                                                {{
                                                                    product.userSelectedProductCount * product.unitPrice
                                                                }}
                                                                {{ product.currency }}
                                                            </s>
                                                        </span>

                                                    </div>
                                                    <div v-else>
                                                        {{ product.unitPrice + product.unitDiscount }}
                                                        {{ product.currency }}
                                                        <span class="text-danger"><s>{{
                                                                product.unitPrice
                                                            }} {{ product.currency }}</s></span>

                                                    </div>
                                                </div>
                                                <div v-else class="d-flex flex-row align-items-center mb-1">
                                                    <div v-if="product.userSelectedProductCount">
                                                        {{ product.userSelectedProductCount }} X
                                                        {{ product.unitPrice }}{{ product.currency }} =
                                                        {{
                                                            product.userSelectedProductCount * product.unitPrice
                                                        }}{{ product.currency }}
                                                    </div>

                                                    <div v-else>
                                                        {{ product.unitPrice }}{{ product.currency }}
                                                    </div>

                                                </div>

                                                <div class="mt-4">
                                                    <div class="form-floating mb-3">
                                                        <input type="number" class="form-control" min="0"
                                                               v-model="product.userSelectedProductCount"
                                                               :max="product.unitCount" id="floatingInput"
                                                               @input="handleInputField($event, product)">
                                                        <label for="floatingInput">Unit count</label>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </section>
        </div>
        <div v-else>
            <NotFound/>
        </div>
    </main>
</template>

<script setup lang="ts">

import {onBeforeMount, ref, watch} from 'vue'
import {useIdentityStore} from "@/stores/identityStore";
import ShopsService from "@/services/shop/ShopsService";
import {useRoute, useRouter} from "vue-router";
import type {IBusiness} from "@/dto/shop/IBusiness";
import NotFound from "@/components/NotFound.vue";
import type {IProduct} from "@/dto/shop/IProduct";
import type {ICreateEditInvoice} from "@/dto/shop/ICreateEditInvoice";
import InvoicesService from "../../services/shop/InvoicesService";
import BusinessIntroduction from "@/components/Shops/BusinessIntroduction.vue";
import type IInvoiceCreateEditProduct from "@/dto/shop/IInvoiceCreateEditProduct";
import InvoiceDetailsCard from "@/components/Shops/InvoiceDetailsCard.vue";
import type {IMessage} from "@/dto/shared/IMessage";
import {MessagePopupTypeEnum} from "@/components/shared/MessagePopupTypeEnum";
import {useMessageStore} from "@/stores/messageStore";
import type {IInvoice} from "@/dto/shop/IInvoice";
import {redirectUserIfIdentityTokenIsNull} from "@/helpers/UserReidrecter";

const identitySore = useIdentityStore();
const shopsService = new ShopsService();
const invoicesService = new InvoicesService();

const route = useRoute();
const router = useRouter();
const businessDetails = ref<IBusiness>()
const showCreationModal = ref<boolean>(false)
const messageStore = useMessageStore();
const invoiceData = ref<IInvoice | undefined>()


const selectedProducts = ref<string[]>([])


const handleInputField = (event: Event, product: IProduct) => {

    let wasAdded: boolean = false

    if (product.userSelectedProductCount > product.unitCount) {
        product.userSelectedProductCount = product.unitCount;
        wasAdded = true
    } else if (product.userSelectedProductCount <= product.unitCount && product.userSelectedProductCount > 0) {

        wasAdded = true
    } else if (product.userSelectedProductCount <= 0) {

        product.userSelectedProductCount = 0;
        wasAdded = false
    }

    if (wasAdded || product.userSelectedProductCount !== 0) {
        // generate code that checks if product id is in array
        const isProductIdPresent = selectedProducts.value.includes(product.id!);

        if (!isProductIdPresent) {
            selectedProducts.value.push(product.id!)
        }
    } else {
        const isProductIdPresent = selectedProducts.value.includes(product.id!);
        if (isProductIdPresent) {
            selectedProducts.value = selectedProducts.value.filter(item => item !== product.id);
        }
    }
}

watch(() => [invoiceData.value, selectedProducts], () => {
})

const createInvoice = async () => {
    const sendableData: ICreateEditInvoice = {
        InvoiceCreateEditProducts: [],
        businessId: businessDetails.value!.id,
    };

    if (businessDetails.value) {
        businessDetails.value.products.forEach((itemProduct) => {
            if (
                itemProduct.id &&
                itemProduct.userSelectedProductCount > 0 &&
                itemProduct.userSelectedProductCount <= itemProduct.unitCount
            ) {
                const invoiceCreateEditProduct: IInvoiceCreateEditProduct = {
                    productId: itemProduct.id,
                    productUnitCount: itemProduct.userSelectedProductCount,
                };
                sendableData.InvoiceCreateEditProducts.push(invoiceCreateEditProduct);
            }
        });

        const identity = identitySore.authenticationJwt;

        if (identity == undefined) {
            console.error('Please log in');
            return;
        }

        if (identity && sendableData && sendableData.InvoiceCreateEditProducts.length > 0) {
            console.log(identity.jwt);
            const result = await invoicesService.createInvoice(identity, sendableData) as ICreateEditInvoice;
            console.log('result', result);
            if (result && result.id) {
                const localInvoiceData = await invoicesService.getInvoice(identity, result.id as string);
                if (localInvoiceData) {
                    invoiceData.value = localInvoiceData
                    console.log("invoice data", invoiceData.value)
                    //     await router.push({ name: 'invoiceAcceptance', params: { id: result.id } });
                }
            }
        }
    }
};


onBeforeMount(async () => {
    await redirectUserIfIdentityTokenIsNull();
    await loadData();
})

const loadData = async () => {

    console.log("Open business details")
    let identity = identitySore.authenticationJwt;


    if (identity === undefined) {
        console.log("jwt is null")
        return;
    }

    if (route.params.id) {
        businessDetails.value = (await shopsService.getBusiness(identity, route.params.id as string))
        console.log("Business details", businessDetails)
    } else {
        console.error("Business id is not initialized")
    }
}


const acceptInput = async () => {
    let identity = identitySore.authenticationJwt;

    if (identity) {
        let result = (await invoicesService.acceptInvoice(identity, invoiceData.value!.id as string, {acceptance: true}))

        if (result == 204) {
            console.log("Invoice status was changed")
            let hider = document.getElementById('closeModal');
            hider!.click()

            await router.push({name: 'invoiceOrder', params: {id: invoiceData.value!.id}});
        } else {
            let message: IMessage = {
                message: "Error occurred when accepting order", status: "",
                type: MessagePopupTypeEnum.Error
            }
            messageStore.addMessage(message)
            console.warn("Error occurred when accepting order")
        }
    } else {
        console.log("Identity problem")
    }
}

const goBackAndEdit = async () => {
    let identity = identitySore.authenticationJwt;

    if (identity) {
        let result = (await invoicesService.deleteInvoice(identity, invoiceData.value!.id as string))

        if (result == 204) {
            console.log("Invoice was deleted")
            invoiceData.value = undefined
            let hider = document.getElementById('closeModal');
            hider!.click()
        } else {
            console.warn("Error occurred when deleting invoice")
        }
    } else {
        console.log("Identity problem")
    }
}

</script>

<style scoped>


</style>
