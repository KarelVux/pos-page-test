<template>
    <!-- Start modal-->
    <section>
        <div class="d-flex">

            <button type="button" class="btn btn-outline-primary" data-bs-toggle="modal"
                    :data-bs-target="'#' + uniqueId">
                Add new Product category
            </button>
            <!-- Modal -->
            <div class="modal fade" :id="uniqueId" data-bs-backdrop="static" data-bs-keyboard="false"
                 tabindex="-1" :aria-labelledby="uniqueId + 'Label'" aria-hidden="true">
                <div class="modal-dialog modal-lg">
                    <div class="modal-content">
                        <div class="modal-header">
                            <h1 class="modal-title fs-5" :id="uniqueId + 'Label'">
                                Add new Product category
                            </h1>
                            <button type="button" class="btn-close" data-bs-dismiss="modal"
                                    aria-label="Close"></button>
                        </div>
                        <div class="modal-body">
                            <div class="row">

                                <div class="col-md-6 mb-3">
                                    <label>Product category name</label>
                                    <input type="text" class="form-control"
                                           v-model="displayData.title">
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close
                            </button>
                            <button type="button" class="btn btn-primary" v-on:click="onSubmit">
                                Add new Product category
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


import {onBeforeMount, ref} from "vue";
import {useIdentityStore} from "@/stores/identityStore";
import {generateRandomString} from "@/helpers/Randomiser";
import {ProductCategoryService} from "@/services/manager/ProductCategoryService";
import type {IManagerProductCategory} from "@/dto/manager/IManagerProductCategory";

const identitySore = useIdentityStore();
const productCategoryService = new ProductCategoryService();
const uniqueId = ref<string>('s' + generateRandomString())
const uniqueHiderId = ref<string>('s' + generateRandomString())


// Define the props and emits
const emits = defineEmits(['update']);

// Create a localData ref to hold the updated values
const displayData = ref<IManagerProductCategory>({
    title: ""
} as IManagerProductCategory)

onBeforeMount(async () => {
})

const onSubmit = async (event: MouseEvent) => {
    event.preventDefault();

    let identity = identitySore.authenticationJwt;

    if (identity === undefined) {
        console.log("jwt is null")
        return;
    }
    let productCategory: IManagerProductCategory | undefined;

    if (displayData.value) {
        productCategory = await productCategoryService.create(identity, displayData.value!)
    }
    if (productCategory) {
        console.log("Product category was  was successfully created")
    } else {
        console.error("Unable to create Product category")
        return
    }
    emits('update');
    let hider = document.getElementById(uniqueHiderId.value);
    hider!.click()
}


</script>