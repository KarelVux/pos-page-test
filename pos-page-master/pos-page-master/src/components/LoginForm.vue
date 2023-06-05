<template>
    <!-- Button trigger modal -->
    <li class="nav-link text-dark " data-bs-toggle="modal" data-bs-target="#loginBackdrop">
        <span class="text-dark custom-mouse-over">Login</span>
    </li>

    <!-- Modal -->
    <div class="modal animate__fade" id="loginBackdrop" data-bs-backdrop="static" data-bs-keyboard="false" tabindex="-1"
         aria-hidden="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title" id="loginBackdropLabel">Login</h5>
                    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
                </div>
                <div class="modal-body">

                    <ul :style=" {display: validationErrors.length == 0 ? 'none' : '' }">
                        <li class="text-danger">{{ validationErrors.length > 0 ? validationErrors[0] : '' }}</li>
                    </ul>
                    <div class="mb-3">
                        <label for="exampleInputEmail1" class="form-label">Email address</label>
                        <input type="email"
                               v-model="loginData.email"
                               class="form-control">
                    </div>
                    <div class="mb-3">
                        <label for="exampleInputPassword1" class="form-label">Password</label>
                        <input type="password"
                               v-model="loginData.password"
                               class="form-control">
                    </div>
                </div>


                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" v-on:click="onSubmit">Log in</button>
                    <p id="loginHider" data-bs-dismiss="modal" style="visibility: hidden" @click="$emit('update:value', false)"></p>
                </div>
            </div>
        </div>
    </div>
</template>


<script setup lang="ts">

import type {ILoginData} from "@/dto/identity/ILoginData";
import {IdentityService} from "@/services/identity/IdentityService";
import {ref} from "vue";


const identityService = new IdentityService();
const loginData = ref<ILoginData>({
    email: "",
    password: "",
} as ILoginData);

const validationErrors = ref<string []>([])

const onSubmit = async (event: MouseEvent) => {
    console.log('onSubmit', event);
    event.preventDefault();

    if (loginData.value.email.length == 0 || loginData.value.password.length == 0) {
        validationErrors.value.push("Bad input values!");
        return;
    }
    // remove errors
    validationErrors.value = [];

    var jwtData = await identityService.login(loginData.value);

    if (jwtData == undefined) {
        // TODO: get error info
        validationErrors.value = ["no jwt"]
        return;
    }

    console.log(jwtData)

    let hider = document.getElementById('loginHider');
    hider!.click()
}

</script>
<style scoped>


.custom-mouse-over {
    cursor: pointer;
}
</style>