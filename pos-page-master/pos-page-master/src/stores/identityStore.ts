import {ref} from 'vue';
import {defineStore} from 'pinia';
import type {IJWTResponse} from "@/dto/identity/IJWTResponse";

export const useIdentityStore = defineStore('identity', () => {
    const authenticationJwt = ref<IJWTResponse>();
    const userName = ref<string | undefined>("")
    return {
        authenticationJwt,
        userName
    };
});


