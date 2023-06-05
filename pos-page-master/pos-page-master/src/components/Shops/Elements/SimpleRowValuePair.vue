<template>
    <div class="row">
        <p class="">
            <span class="">
                <strong>{{props.strongText}}</strong>
           </span>
              <span>{{ props.normalText }}</span>
        </p>
    </div>
</template>


<script lang="ts" setup>


const props = defineProps({
    strongText: String,
    normalText: String,

})

</script>
