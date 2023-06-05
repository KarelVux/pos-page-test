<template>
    <div class="container" v-if="managerBusinessData">
        <div class="card my-1">
            <div class="card-body ">
                <!-- Start Business info-->
                <div>
                    <BusinessIntroduction :businessDetails="managerBusinessData">
                        <div class="d-flex  flex-column">
                            <div class="d-flex flex-row mb-1">
                                <div>

                                    <button class="btn btn-primary me-2" @click="loadPageData"><i
                                        class="bi bi-arrow-clockwise me-2"></i>Refresh page data
                                    </button>
                                </div>
                                <div>
                                    <BusinessCreateEditModal
                                        :businessData="managerBusinessData"
                                        :create="false"
                                        :settlements="settlements"
                                        :businessCategories="businessCategories"
                                        @update="updateObjectData"/>
                                </div>
                            </div>

                            <div v-if="managerBusinessData.id" class="d-flex flex-row">
                                <div class="d-flex"
                                     v-if="managerBusinessData.picturePath ||managerBusinessData.picturePath?.length >= 0 ">
                                    <button type="button" class="btn btn-outline-danger"
                                            @click="deleteBusinessImage(managerBusinessData.id)">
                                        Delete picture
                                    </button>
                                </div>
                                <div v-else>
                                    <div class="mb-3">
                                        <input class="form-control  w-100" type="file" accept="image/*"
                                               @change="handleFileInputChange">
                                        <button type="button" class="btn btn-outline-success w-100 mt-1"
                                                :disabled="selectedImage == undefined"
                                                @click="uploadBusinessImage(managerBusinessData.id)">
                                            Add picture
                                        </button>

                                    </div>


                                </div>
                            </div>

                        </div>


                    </BusinessIntroduction>
                </div>

                <!-- End Business info-->
            </div>
        </div>
        <!-- Start Product info-->

        <div class="card my-1">
            <div class="card-body ">
                <div v-if="managerBusinessData">
                    <div class="card-title d-flex  justify-content-center py-2">

                        <h2 class="d-flex me-2"> Business products</h2>
                        <ProductCreateEditModal
                            class="d-flex ms-2"
                            :productData="{ } as IManagerProduct"
                            :businessId="businessId!"
                            :create="true"
                            :productCategories="productCategories"
                            @update="updateObjectData"/>

                        <ProductCategoryCreateModal
                            class="d-flex ms-2"
                            @update="updateObjectData"
                        />
                    </div>

                    <table v-if=" managerBusinessData && managerBusinessData.products"
                           class="table table-striped table-sm ">
                        <thead>
                        <tr>
                            <th>
                            </th>
                            <th>
                                Name
                            </th>
                            <th>
                                Description
                            </th>
                            <th>
                                Category
                            </th>
                            <th>
                                Unit Count
                            </th>
                            <th>
                                Unit Price
                            </th>
                            <th>
                                Unit Discount
                            </th>
                            <th>
                                Tax percent
                            </th>
                            <th>
                                Currency
                            </th>
                            <th>
                                Frozen
                            </th>

                        </tr>
                        </thead>
                        <tbody class="">
                        <tr v-for="item  in managerBusinessData.products" :key="item.id" class="">
                            <td>
                                <div class="">

                                    <img v-if="!item.picturePath || item.picturePath.length <= 0"
                                         src="https://images.unsplash.com/photo-1528698827591-e19ccd7bc23d?ixlib=rb-4.0.3&ixid=MnwxMjA3fDB8MHxwaG90by1wYWdlfHx8fGVufDB8fHx8&auto=format&fit=crop&w=1176&q=80"
                                         class="img-fluid" alt=""/>
                                    <img v-else
                                         :src="item.picturePath"
                                         class="img-fluid" alt=""/>
                                </div>

                                <ProductCreateEditModal
                                    :productData="item"
                                    :businessId="businessId!"
                                    :create="false"
                                    :productCategories="productCategories"
                                    @update="updateObjectData"
                                />

                                <div v-if="item && item.id" class="d-flex flex-row">
                                    <div class="d-flex"
                                         v-if="item.picturePath ||item.picturePath?.length >= 0 ">
                                        <button type="button" class="btn btn-outline-danger "
                                                @click="deleteProductImage(item.id)">
                                            Delete picture
                                        </button>

                                    </div>
                                    <div v-else>
                                        <div class="mb-3">
                                            <input class="form-control  w-100" type="file" accept="image/*"
                                                   @change="handleFileInputChange">
                                            <button type="button" class="btn btn-outline-success w-100 mt-1"
                                                    :disabled="selectedImage == undefined"
                                                    @click="uploadProductImage(item.id)">
                                                Add picture
                                            </button>
<!--
                                            <button type="button" class="btn btn-outline-info w-100 mt-1"
                                                    @click="clearUploadedFileHandler">
                                                Clear uploaded data
                                            </button>
                                            -->

                                        </div>
                                    </div>
                                </div>
                            </td>
                            <td>
                                {{ item.name }}
                            </td>
                            <td>
                                {{ item.description }}
                            </td>
                            <td>
                                {{ item.productCategory.title }}
                            </td>
                            <td>
                                {{ item.unitCount }}
                            </td>
                            <td>
                                {{ item.unitPrice }}
                            </td>
                            <td>
                                {{ item.unitDiscount }}
                            </td>
                            <td>
                                {{ item.taxPercent }}
                            </td>
                            <td>
                                {{ item.currency }}
                            </td>
                            <td>
                                {{ item.frozen }}
                            </td>

                        </tr>
                        </tbody>
                    </table>
                </div>
            </div>

        </div>
        <!-- End Product info-->

        <!-- Start invoices -->
        <div class="card my-2" v-if="openInvoices.length > 0">
            <div class="card-body mb-4">
                <h2 class="card-title text-center py-2">
                    Active invoice orders
                </h2>
                <table class="table border-bottom border-gray-200 mt-3">
                    <thead>
                    <tr>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Invoice ID</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Creation Time</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Username</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Payment Completed</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Products</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Subtotal</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Tax</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Total</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Acceptance status</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Order Status</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Actions</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="openInvoice in openInvoices " :key="openInvoice.id">
                        <td class="px-1">
                            <InvoiceDetailsModal :invoiceData="openInvoice"/>
                        </td>
                        <td class=" px-1">
                            {{ getFormattedDate(openInvoice.creationTime) }}
                        </td>
                        <td class=" px-1">{{ openInvoice.userName }}</td>

                        <td class=" px-1">{{ openInvoice.paymentCompleted }}</td>

                        <td class=" px-1">
                            <p v-for="(invoiceRow, index) in openInvoice.invoiceRows" :key="index">
                                {{ invoiceRow.productUnitCount }} X {{ invoiceRow.productName }} =
                                {{ invoiceRow.finalProductPrice }}
                            </p>
                        </td>
                        <td class=" px-1">{{ openInvoice.totalPriceWithoutTax }}</td>
                        <td class=" px-1">{{ openInvoice.taxAmount }}</td>
                        <td class=" px-1">{{ openInvoice.finalTotalPrice }}</td>
                        <td class=" px-1">
                            <p v-if="openInvoice.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.Unknown">
                                Unknown
                            </p>
                            <p v-else-if="openInvoice.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.UserAccepted">
                                User accepted
                            </p>

                            <p v-else-if="openInvoice.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.BusinessAccepted">
                                Business accepted
                            </p>
                            <p v-else-if="openInvoice.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.BusinessRejected">
                                Business rejected
                            </p>

                        </td>
                        <td class=" px-1">
                            <p v-if="openInvoice.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.GivenToClient">
                                Given to client
                            </p>
                            <p v-else-if="openInvoice.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.Ready">
                                Ready
                            </p>
                            <p v-else-if="openInvoice.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.BusinessIsPreparing">
                                Preparing
                            </p>
                            <p v-else-if="openInvoice.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.Unknown">
                                Needs business acceptance
                            </p>

                        </td>
                        <td class=" px-1">

                            <button
                                v-if="openInvoice.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.Unknown || openInvoice.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.UserAccepted"
                                type="button" class="btn btn-outline-primary w-100"
                                @click="acceptInvoice(openInvoice)">
                                Accept
                            </button>

                            <button
                                v-if="openInvoice.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.BusinessIsPreparing && openInvoice.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.BusinessAccepted"
                                type="button" class="btn btn-outline-primary w-100"
                                @click="moveToReadyStatus(openInvoice)">
                                Move order to ready status
                            </button>

                            <button
                                v-if="openInvoice.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.Ready && openInvoice.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.BusinessAccepted"
                                type="button" class="btn btn-outline-primary w-100"
                                @click="moveToGivenToClientStatus(openInvoice)">
                                Move order to given to client status
                            </button>


                            <button type="button" class="btn btn-outline-danger   w-100"
                                    @click="rejectInvocie(openInvoice)">
                                Decline
                            </button>
                            <button type="button" class="btn btn-outline-info  w-100"
                                    @click="paymentCompleted(openInvoice)">
                                Payment completed
                            </button>
                        </td>
                    </tr>
                    </tbody>
                </table>
            </div>
        </div>

        <div class="card my-2" v-if="closedInvoices.length > 0">
            <div class="card-body mb-4">
                <h2 class="card-title text-center py-2">
                    Invoice history
                </h2>
                <table class="table border-bottom border-gray-200 mt-3">
                    <thead>
                    <tr>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Invoice ID</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Creation Time</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Username</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Payment Completed</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Products</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Subtotal</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Tax</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Total</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Acceptance status</th>
                        <th scope="col" class="fs-sm text-dark text-uppercase-bold-sm  px-1">Order Status</th>
                    </tr>
                    </thead>
                    <tbody>
                    <tr v-for="closedInvoice in closedInvoices " :key="closedInvoice.id">
                        <td class="px-1">
                            <InvoiceDetailsModal
                                :invoiceData="closedInvoice"
                                @click="loadOrderFeedbackData(closedInvoice.orderId)"
                            >
                                <div class="card mt-2">
                                    <div class="card-body p-5" v-if="orderFeedbackData">
                                        <h2>
                                            Here is the previously submitted order feedback
                                        </h2>
                                        <div>
                                            <p v-if="orderFeedbackData.title"><strong>Title</strong>: {{ orderFeedbackData.title }}</p>
                                            <p><strong>Rating</strong>: {{ orderFeedbackData.rating }}</p>
                                            <p><strong>Description</strong></p>
                                            <p>{{ orderFeedbackData.description }}</p>
                                        </div>
                                    </div>
                                </div>
                            </InvoiceDetailsModal>
                        </td>
                        <td class=" px-1">
                            {{ getFormattedDate(closedInvoice.creationTime) }}
                        </td>
                        <td class=" px-1">{{ closedInvoice.userName }}</td>

                        <td class=" px-1">{{ closedInvoice.paymentCompleted }}</td>

                        <td class=" px-1">
                            <p v-for="(invoiceRow, index) in closedInvoice.invoiceRows" :key="index">
                                {{ invoiceRow.productUnitCount }} X {{ invoiceRow.productName }} =
                                {{ invoiceRow.finalProductPrice }}
                            </p>
                        </td>
                        <td class=" px-1">{{ closedInvoice.totalPriceWithoutTax }}</td>
                        <td class=" px-1">{{ closedInvoice.taxAmount }}</td>
                        <td class=" px-1">{{ closedInvoice.finalTotalPrice }}</td>
                        <td class=" px-1">
                            <p v-if="closedInvoice.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.Unknown">
                                Unknown
                            </p>
                            <p v-else-if="closedInvoice.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.UserAccepted">
                                User accepted
                            </p>

                            <p v-else-if="closedInvoice.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.BusinessAccepted">
                                Business accepted
                            </p>
                            <p v-else-if="closedInvoice.invoiceAcceptanceStatus == InvoiceAcceptanceStatusEnum.BusinessRejected">
                                Business rejected
                            </p>

                        </td>
                        <td class=" px-1">
                            <p v-if="closedInvoice.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.GivenToClient">
                                Given to client
                            </p>
                            <p v-else-if="closedInvoice.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.Ready">
                                Ready
                            </p>
                            <p v-else-if="closedInvoice.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.BusinessIsPreparing">
                                Preparing
                            </p>
                            <p v-else-if="closedInvoice.order.orderAcceptanceStatus == OrderAcceptanceStatusEnum.Unknown">
                                Needs business acceptance
                            </p>
                        </td>
                    </tr>
                    </tbody>
                </table>

            </div>
        </div>
        <div v-else>
            Sorry there are not any open invoices
        </div>
        <!-- End invoices -->
    </div>
    <div v-else>
        <LoadingData/>
    </div>
</template>


<script lang="ts" setup>
import {ManagerBusinessService} from "@/services/manager/ManagerBusinessService";
import {useIdentityStore} from "@/stores/identityStore";
import {onBeforeMount, ref, watch} from "vue";
import type {IManagerBusiness} from "@/dto/manager/IManagerBusiness";
import {useRoute} from "vue-router";
import {useMessageStore} from "@/stores/messageStore";
import BusinessIntroduction from "@/components/Shops/BusinessIntroduction.vue";
import LoadingData from "@/components/shared/LoadingData.vue";
import BusinessCreateEditModal from "@/components/manager/BusinessCreateEditModal.vue";
import ProductCreateEditModal from "@/components/manager/ProductCreateEditModal.vue";
import {InvoiceService} from "@/services/manager/InvoiceService";
import type {IManagerInvoice} from "@/dto/manager/IManagerInvoice";
import {getFormattedDate} from "@/helpers/UnifiedFormatter";
import {OrderAcceptanceStatusEnum} from "@/dto/enums/OrderAcceptanceStatusEnum";
import {InvoiceAcceptanceStatusEnum} from "@/dto/enums/InvoiceAcceptanceStatusEnum";
import {MessagePopupTypeEnum} from "@/components/shared/MessagePopupTypeEnum";
import type {IMessage} from "@/dto/shared/IMessage";
import type {IManagerProduct} from "@/dto/manager/IManagerProduct";
import InvoiceDetailsModal from "@/components/manager/InvoiceDetailsModal.vue";
import {redirectUserIfIdentityTokenIsNull} from "@/helpers/UserReidrecter";
import type {IManagerSettlement} from "../../dto/manager/IManagerSettlement";
import type {IManagerBusinessCategory} from "../../dto/manager/IManagerBusinessCategory";
import {SettlementService} from "../../services/manager/SettlementService";
import ProductCategoryCreateModal from "@/components/manager/ProductCategoryCreateModal.vue";
import type {IManagerProductCategory} from "@/dto/manager/IManagerProductCategory";
import {ProductCategoryService} from "@/services/manager/ProductCategoryService";
import {BusinessCategoriesService} from "@/services/manager/BusinessCategoriesService";
import type {IManagerInvoiceOrderLimitedEdit} from "@/dto/manager/IManagerInvoiceOrderLimitedEdit";
import OrderFeedbackService from "@/services/manager/OrderFeedbackService";
import type {IManagerOrderFeedback} from "@/dto/manager/IManagerOrderFeedback";
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";
import {PictureService} from "@/services/manager/PictureService";
import {BusinessPictureService} from "@/services/manager/BusinessPictureService";
import {ProductPictureService} from "@/services/manager/ProductPictureService";

const managerBusinessService = new ManagerBusinessService();
const invoiceService = new InvoiceService();
const settlementService = new SettlementService();
const businessCategoriesService = new BusinessCategoriesService();
const productCategoryService = new ProductCategoryService();
const orderFeedbackService = new OrderFeedbackService();
const pictureService = new PictureService();
const businessPictureService = new BusinessPictureService();
const productPictureService = new ProductPictureService();

const identitySore = useIdentityStore();
const messageStore = useMessageStore();
const route = useRoute();

const managerBusinessData = ref<IManagerBusiness>();
const businessId = ref<string>()
const openInvoices = ref<IManagerInvoice[]>([]);
const closedInvoices = ref<IManagerInvoice[]>([]);
const settlements = ref<IManagerSettlement[]>([]);
const businessCategories = ref<IManagerBusinessCategory[]>([]);
const productCategories = ref<IManagerProductCategory[]>([]);
const orderFeedbackData = ref<IManagerOrderFeedback>();
const selectedImage = ref<File | undefined>();

watch(() => managerBusinessData.value, async () => {
});

watch(() => [openInvoices.value, closedInvoices.value], async () => {
});

watch(() => [productCategories.value], async () => {
    console.log("productCategories.value 232323", productCategories.value)
});


// Method to update objectData
const updateObjectData = async () => {
    // managerBusinessData.value = businessData;
    await loadPageData();
};

onBeforeMount(async () => {
    await redirectUserIfIdentityTokenIsNull();

    businessId.value = route.params.id as string
    await loadPageData();

})

const acceptInvoice = async (invoice: IManagerInvoice) => {
    let identity = identitySore.authenticationJwt

    let invoiceChangeableData: IManagerInvoiceOrderLimitedEdit = {
        id: invoice.id,
        invoiceAcceptanceStatus: InvoiceAcceptanceStatusEnum.BusinessAccepted,
        orderAcceptanceStatus: invoice.order.orderAcceptanceStatus,
        paymentCompleted: invoice.paymentCompleted
    }

    invoiceChangeableData.orderAcceptanceStatus = OrderAcceptanceStatusEnum.BusinessIsPreparing;

    if (identity && invoiceChangeableData.id) {
        let returnableInvoice = await invoiceService.doPartialUpdateWithLimitedData(identity, invoiceChangeableData.id, invoiceChangeableData)
        if (returnableInvoice) {
            console.log("Successfully changed")
            var message: IMessage = {
                message: "Status was successfully changed",
                type: MessagePopupTypeEnum.Info
            }
            messageStore.addMessage(message)
        } else {
            messageStore.addMessage({
                message: "Unable to edit invoice ", status: "", type: MessagePopupTypeEnum.Error

            })
        }

        await loadInvoiceData();
    } else {
        console.log("Problem with identity or invoice id")
    }
}


const rejectInvocie = async (invoice: IManagerInvoice) => {
    let identity = identitySore.authenticationJwt

    let invoiceChangeableData: IManagerInvoiceOrderLimitedEdit = {
        id: invoice.id,
        invoiceAcceptanceStatus: InvoiceAcceptanceStatusEnum.BusinessRejected,
        orderAcceptanceStatus: invoice.order.orderAcceptanceStatus,
        paymentCompleted: invoice.paymentCompleted
    }

    invoiceChangeableData.orderAcceptanceStatus = OrderAcceptanceStatusEnum.Closed;

    if (identity && invoiceChangeableData.id) {
        let returnableInvoice = await invoiceService.doPartialUpdateWithLimitedData(identity, invoiceChangeableData.id, invoiceChangeableData)
        if (returnableInvoice) {
            console.log("Successfully changed")
            var message: IMessage = {
                message: "Status was successfully changed",
                type: MessagePopupTypeEnum.Info
            }
            messageStore.addMessage(message)
        } else {
            messageStore.addMessage({
                message: "Unable to edit invoice ", status: "", type: MessagePopupTypeEnum.Error

            })
        }
        await loadInvoiceData();
    } else {
        console.log("Problem with identity or invoice id")
    }
}

const moveToReadyStatus = async (invoice: IManagerInvoice) => {
    let identity = identitySore.authenticationJwt

    let invoiceChangeableData: IManagerInvoiceOrderLimitedEdit = {
        id: invoice.id,
        invoiceAcceptanceStatus: invoice.invoiceAcceptanceStatus,
        orderAcceptanceStatus: invoice.order.orderAcceptanceStatus,
        paymentCompleted: invoice.paymentCompleted
    }

    invoiceChangeableData.orderAcceptanceStatus = OrderAcceptanceStatusEnum.Ready;

    if (identity && invoiceChangeableData.id) {
        let returnableInvoice = await invoiceService.doPartialUpdateWithLimitedData(identity, invoiceChangeableData.id, invoiceChangeableData)
        if (returnableInvoice) {
            console.log("Successfully changed")
            var message: IMessage = {
                message: "Status was successfully changed ",
                type: MessagePopupTypeEnum.Info
            }
            messageStore.addMessage(message)
        } else {
            messageStore.addMessage({
                message: "Unable to edit invoice ", status: "", type: MessagePopupTypeEnum.Error

            })
        }
        await loadInvoiceData();
    } else {
        console.log("Problem with identity or invoice id")
    }
}

const moveToGivenToClientStatus = async (invoice: IManagerInvoice) => {
    let identity = identitySore.authenticationJwt

    let invoiceChangeableData: IManagerInvoiceOrderLimitedEdit = {
        id: invoice.id,
        invoiceAcceptanceStatus: invoice.invoiceAcceptanceStatus,
        orderAcceptanceStatus: invoice.order.orderAcceptanceStatus,
        paymentCompleted: invoice.paymentCompleted
    }

    invoiceChangeableData.orderAcceptanceStatus = OrderAcceptanceStatusEnum.GivenToClient;

    if (identity && invoiceChangeableData.id) {
        let returnableInvoice = await invoiceService.doPartialUpdateWithLimitedData(identity, invoiceChangeableData.id, invoiceChangeableData)
        if (returnableInvoice) {
            console.log("Successfully changed")
            var message: IMessage = {
                message: "Status was successfully changed ",
                type: MessagePopupTypeEnum.Info
            }
            messageStore.addMessage(message)
        } else {
            messageStore.addMessage({
                message: "Unable to edit invoice ", status: "", type: MessagePopupTypeEnum.Error
            })
        }
        await loadInvoiceData();
    } else {
        console.log("Problem with identity or invoice id")
    }
}
const paymentCompleted = async (invoice: IManagerInvoice) => {
    let identity = identitySore.authenticationJwt

    let invoiceChangeableData: IManagerInvoiceOrderLimitedEdit = {
        id: invoice.id,
        invoiceAcceptanceStatus: invoice.invoiceAcceptanceStatus,
        orderAcceptanceStatus: invoice.order.orderAcceptanceStatus,
        paymentCompleted: true
    }

    if (identity && invoiceChangeableData.id) {
        let returnableInvoice = await invoiceService.doPartialUpdateWithLimitedData(identity, invoiceChangeableData.id, invoiceChangeableData)
        if (returnableInvoice) {
            console.log("Successfully changed")
            var message: IMessage = {
                message: "Status was successfully changed ",
                type: MessagePopupTypeEnum.Info
            }
            messageStore.addMessage(message)
        } else {
            messageStore.addMessage({
                message: "Unable to edit invoice ", status: "", type: MessagePopupTypeEnum.Error

            })
        }
        await loadInvoiceData();
    } else {
        console.log("Problem with identity or invoice id")
    }
}

const loadPageData = async () => {
    let identity = identitySore.authenticationJwt

    if (identity) {
        let business = await managerBusinessService.getById(identity, businessId.value!)
        if (business) {
            managerBusinessData.value = business;
        } else {
            messageStore.addMessage({
                message: "Unable to find business " + route.params.id, status: "", type: MessagePopupTypeEnum.Error

            })
        }

        productCategories.value = (await productCategoryService.getAll(identity))!
        settlements.value = (await settlementService.getAll(identity))!
        businessCategories.value = (await businessCategoriesService.getAll(identity))!

        console.log("product categories on  page load called out", productCategories)

        await loadInvoiceData();
    }
}

const loadInvoiceData = async () => {
    let identity = identitySore.authenticationJwt

    if (identity) {
        let invoices = await invoiceService.getBusinessInvoices(identity, businessId.value!)
        if (invoices) {
            console.log("Got invoices", invoices)
            closedInvoices.value = []
            openInvoices.value = []
            invoices.forEach(function (item) {
                if (item.invoiceAcceptanceStatus === InvoiceAcceptanceStatusEnum.BusinessRejected || (item.order.orderAcceptanceStatus === OrderAcceptanceStatusEnum.GivenToClient && item.paymentCompleted)) {
                    closedInvoices.value.push(item)
                } else {
                    openInvoices.value.push(item)
                }
            });


            console.log("closed invoice", closedInvoices)
            console.log("open invoice", openInvoices)
        } else {
            console.warn("Error occurred when checking invoices ")
        }


    }
}

const loadOrderFeedbackData = async (orderId: string) => {
    let identity = identitySore.authenticationJwt
    if (identity) {
        let orderFeedback = await orderFeedbackService.getOrderFeedbackViaOrderId(identity, orderId)
        if (orderFeedback) {
            orderFeedbackData.value = orderFeedback
            console.log("open order feedback", orderFeedbackData.value)
        } else {
            orderFeedbackData.value = undefined
            console.warn("Error occurred when checking order feedback ")
        }
    } else {
        console.warn("Problems with identity")
    }
}

const handleFileInputChange = (event: Event) => {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
        selectedImage.value = target.files[0];
    }
};


/*
const clearUploadedFileHandler = (event: Event) => {
    const target = event.target as HTMLInputElement;
    if (target.files && target.files.length > 0) {
        target.files = null
        selectedImage.value = undefined;
    }
};
*/
const uploadBusinessImage = async (businessId: string) => {
        {
            if (!selectedImage.value) return;
            let identity = identitySore.$state.authenticationJwt as IJWTResponse;
            if (identitySore.$state.authenticationJwt && businessId) {
                let result = await pictureService.postBusinessPicture(identity, businessId, selectedImage.value);
                if (result) {
                    console.log("Successfully uploaded business picture");
                    selectedImage.value = undefined;
                    await loadPageData();
                } else {
                    console.log("Unable to upload business picture");
                }
            } else {
                console.log("Unable to authenticate  or business id is not provided");
            }
        }
    }
;

const deleteBusinessImage = async (businessId: string) => {
        {
            console.log("delete business image called")
            let identity = identitySore.$state.authenticationJwt as IJWTResponse;
            if (identitySore.$state.authenticationJwt && businessId) {
                let businessPictureDataResponse = await businessPictureService.getBusinessPicture(identity, businessId)

                if (businessPictureDataResponse) {
                    let result = await pictureService.deletePicture(identity, businessPictureDataResponse.pictureId);
                    if (result) {
                        console.log("Successfully deleted business picture");
                        selectedImage.value = undefined;
                        await loadPageData();
                    } else {
                        console.log("Unable to delete business picture");
                    }
                } else {
                    console.log("Unable get business picture");
                }
            } else {
                console.log("Unable to authenticate  or business id is not provided");
            }
        }
    }
;


const uploadProductImage = async (productId: string) => {
        {
            if (!selectedImage.value) return;
            let identity = identitySore.$state.authenticationJwt as IJWTResponse;
            if (identitySore.$state.authenticationJwt && productId) {
                let result = await pictureService.postProductPicture(identity, productId, selectedImage.value);
                if (result) {
                    console.log("Successfully uploaded product picture");
                    selectedImage.value = undefined;
                    await loadPageData();
                } else {
                    console.log("Unable to upload product picture");
                }
            } else {
                console.log("Unable to authenticate  or product id is not provided");
            }
        }
    }
;

const deleteProductImage = async (productId: string) => {
        {
            console.log("delete product image called")
            let identity = identitySore.$state.authenticationJwt as IJWTResponse;
            if (identitySore.$state.authenticationJwt && productId) {
                let productPictureDataResponse = await productPictureService.getProductPicturePicture(identity, productId)

                if (productPictureDataResponse) {
                    let result = await pictureService.deletePicture(identity, productPictureDataResponse.pictureId);
                    if (result) {
                        console.log("Successfully deleted product picture");
                        selectedImage.value = undefined;
                        await loadPageData();
                    } else {
                        console.log("Unable to delete product picture");
                    }
                } else {
                    console.log("Unable get product picture");
                }
            } else {
                console.log("Unable to authenticate  or product id is not provided");
            }
        }
    }
;


</script>


<style scoped>
.table-responsive {
    overflow-y: scroll;
    height: 200px;
}
</style>