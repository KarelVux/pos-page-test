<template>
    <!-- Button trigger modal -->
    <li class="nav-link text-dark " data-bs-toggle="modal" data-bs-target="#registerBackdrop">
        <span class="text-dark custom-mouse-over">Register</span>
    </li>

    <!-- Modal -->
    <div class="modal animate__fade" id="registerBackdrop" data-bs-backdrop="static" data-bs-keyboard="false"
         tabindex="-1"
         aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="registerBackdropLabel">Register</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">

                    <ul :style=" {display: validationErrors.length == 0 ? 'none' : '' }">
                        <li class="text-danger">{{ validationErrors.length > 0 ? validationErrors[0] : '' }}</li>
                    </ul>
                    <div class="mb-3">
                        <label for="exampleInputEmail1" class="form-label">Email address</label>
                        <input type="email"
                               v-model="registerData.email"
                               class="form-control">
                    </div>
                    <div class="mb-3">
                        <label for="exampleInputPassword1" class="form-label">Password</label>
                        <input type="password"
                               v-model="registerData.password"
                               class="form-control">
                    </div>
                    <div class="mb-3">
                        <label for="exampleInputPassword1" class="form-label">Confirm password</label>
                        <input type="password"
                               v-model="registerData.confirmPassword"
                               class="form-control">
                    </div>
                </div>


                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" v-on:click="onSubmit">Register</button>
                    <p id="registerHider" data-bs-dismiss="modal" style="visibility: hidden" @click="$emit('update:value', false)"></p>
                </div>
            </div>
        </div>
    </div>
</template>


<script setup lang="ts">

import {IdentityService} from "@/services/identity/IdentityService";
import {ref} from "vue";
import type {IRegisterData} from "@/dto/identity/IRegisterData";


const identityService = new IdentityService();
const registerData = ref<IRegisterData>({
    password: "",
    confirmPassword: "",
    email: "",
} as IRegisterData);

const validationErrors = ref<string []>([])
const emits = defineEmits(['update:value']);

const onSubmit = async (event: MouseEvent) => {
    console.log('onSubmit', event);
    event.preventDefault();

    if (registerData.value.email.length == 0 || registerData.value.password.length == 0 || registerData.value.password != registerData.value.confirmPassword) {
        validationErrors.value.push("Bad input values!");
        return;
    }
    // remove errors
    validationErrors.value = [];

    var jwtData = await identityService.register(registerData.value);

    if (jwtData == undefined) {
        // TODO: get error info
        validationErrors.value.push("no jwt");
        return;
    }

    console.log(jwtData)

    // close modal
    let hider = document.getElementById('registerHider');
    hider!.click();
}

</script>
<style scoped>


.custom-mouse-over {
    cursor: pointer;
}
</style>