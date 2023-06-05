<template>
  <div class="progress" role="progressbar" aria-label="Success example" :aria-valuenow="props.progressbarWidth" aria-valuemin="0"
       aria-valuemax="101">
    <div class="progress-bar bg-success"
         :style="{width: props.progressbarWidth + '%'}">
    </div>
  </div>


<!--
  <div class="progress" role="progressbar" aria-label="Success example" aria-valuenow="35" aria-valuemin="0"
       aria-valuemax="100">
    <div class="progress-bar bg-success"
         style="width: 35%"></div>
  </div>
  -->
</template>


<script lang="ts" setup>
import {watch} from "vue";
interface IProps {
  progressbarWidth: number
}

const props = defineProps<IProps>()

watch(() => props.progressbarWidth,  () => {
  // do something when the data changes
});


</script>