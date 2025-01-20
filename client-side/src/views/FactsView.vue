<script setup lang="ts">
import { useConfigStore } from '@/stores';
import { storeToRefs } from 'pinia';
import { inject, onMounted, ref, watch } from 'vue';
import { enumToObject } from '@/utils/enumUtil';
import { FactOrder } from '@/enums';
import type { APIFact, Paginated } from '@/typings';
import { HttpStatusCode, type AxiosInstance } from 'axios';
import { assertType } from '@/helpers/typeAssertHelper';
import BtnLike from '@/components/buttons/BtnLike.vue';
import BtnDislike from '@/components/buttons/BtnDislike.vue';
import { useLocalStorageRef } from '@/utils/refUtil';

const acpOrderItems = enumToObject(FactOrder);
/// GUARANTEE: the 'axios' dependency is provided by the axios plugin
const axios = inject<AxiosInstance>('axios')!;

const { showAppBar } = storeToRefs(useConfigStore());

const acpOrder = useLocalStorageRef<FactOrder>('acp-order', FactOrder.Alphabetical);
const ckbDescending = useLocalStorageRef<boolean>('ckb-descending,order', false);
const jsonApiFacts = ref<Paginated<APIFact>>();
const pgnApiFacts = ref<number>(1);
const sklApiFactsIf = ref<boolean>(false);

async function btnDislikeClick(apiFact: APIFact) {
  /// GUARANTEE: These lines are only called when an existing 'apiFact' recieves an interaction
  const index = jsonApiFacts.value!.items.findIndex((a) => a.id === apiFact.id);

  if (index !== -1)
    /// GUARANTEE: These lines are only called when an existing 'apiFact' recieves an interaction
    jsonApiFacts.value!.items[index].dislikeCount++;

  const res = await axios.post(`Fact/${apiFact.id}/Dislike`);
  if (res?.status !== HttpStatusCode.Ok)
    /// GUARANTEE: These lines are only called when an existing 'apiFact' recieves an interaction
    jsonApiFacts.value!.items[index].likeCount = apiFact.dislikeCount;
}

async function btnLikeClick(apiFact: APIFact) {
  /// GUARANTEE: These lines are only called when an existing 'apiFact' recieves an interaction
  const index = jsonApiFacts.value!.items.findIndex((a) => a.id === apiFact.id);

  if (index !== -1)
    /// GUARANTEE: These lines are only called when an existing 'apiFact' recieves an interaction
    jsonApiFacts.value!.items[index].likeCount++;

  const res = await axios.post(`Fact/${apiFact.id}/Like`);
  if (res?.status !== HttpStatusCode.Ok)
    /// GUARANTEE: These lines are only called when an existing 'apiFact' recieves an interaction
    jsonApiFacts.value!.items[index].likeCount = apiFact.likeCount;
}

async function updateJsonApiFacts() {
  const params = {
    pageIndex: pgnApiFacts.value - 1,
    descending: ckbDescending.value,
    order: acpOrder.value
  };

  sklApiFactsIf.value = true;

  try {
    const res = await axios.get('Fact', { params });
    if (res?.status === HttpStatusCode.Ok) {
      const paginated = assertType<Paginated<APIFact>>({ paginated: res.data });
      jsonApiFacts.value = paginated;
    }
  } finally {
    sklApiFactsIf.value = false;
  }
}

onMounted(() => {
  showAppBar.value = true;
  updateJsonApiFacts();
});

watch([acpOrder, pgnApiFacts, ckbDescending], ([valAcpOrder]) =>
  typeof valAcpOrder === 'number'
    ? updateJsonApiFacts()
    : jsonApiFacts.value = undefined
);
</script>

<template>
  <div class="facts-view">
    <v-card class="crd-styling">
      <v-icon style="position: absolute; left: 4px; top: -2px" color="surface" size="40">
        {{ 'mdi-cat' }}
      </v-icon>
      <v-icon style="position: absolute; right: 4px; top: -2px" color="surface" size="40">
        {{ 'mdi-cat' }}
      </v-icon>
      <v-card-title class="text-center bg-primary py-2">LIST</v-card-title>
      <div class="justify-center flex-column d-flex ga-2 ma-2">
        <div class="d-flex ga-2">
          <v-autocomplete
            prepend-icon="mdi-segment"
            :items="acpOrderItems"
            item-value="value"
            v-model="acpOrder"
            item-title="key"
            label="Order"
          />
          <v-switch v-model="ckbDescending" label="Desc. order" color="error" inset />
        </div>

        <v-card class="crd-table-styling" variant="tonal">
          <v-table class="tbl-styling">
            <thead>
              <tr>
                <th class="bg-secondary">TEXT</th>
                <th class="text-center bg-secondary" style="width: 126px">POPULARITY</th>
                <th class="text-center bg-secondary" style="width: 126px">OCCURRENCES</th>
                <th class="text-center bg-secondary" style="width: 112px">ACTIONS</th>
              </tr>
            </thead>
            <v-fade-transition mode="out-in">
              <v-skeleton-loader v-if="sklApiFactsIf" />
              <tbody mode="out-in" v-else>
                <tr v-for="(apiFact, index) in jsonApiFacts?.items" :key="index">
                  <td>{{ apiFact.text }}</td>
                  <td class="text-center">{{ apiFact.likeCount - apiFact.dislikeCount }}</td>
                  <td class="text-center">{{ apiFact.occurrenceCount }}</td>
                  <td>
                    <BtnLike
                      @click="btnLikeClick(apiFact)"
                      :amount="apiFact.likeCount"
                      text="Like this cat fact"
                    />
                    <BtnDislike
                      @click="btnDislikeClick(apiFact)"
                      :amount="apiFact.dislikeCount"
                      text="Dislike this cat fact"
                    />
                  </td>
                </tr>
              </tbody>
            </v-fade-transition>
          </v-table>
        </v-card>

        <div class="text-medium-emphasis text-center">
          <v-pagination
            :length="jsonApiFacts?.totalPages"
            :disabled="sklApiFactsIf"
            v-model="pgnApiFacts"
            variant="outlined"
            total-visible="4"
          />
          {{ jsonApiFacts ? `${jsonApiFacts.totalItems} total items` : '...' }}
        </div>
      </div>
    </v-card>
  </div>
</template>

<style scoped>
.crd-styling {
  height: calc(100dvh - 4rem - 8px * 6);
}

.crd-table-styling {
  height: calc(100dvh - 8px * 35);
}

.facts-view {
  margin: 2rem;
}

.tbl-styling {
  height: 100%;
}

@media (max-width: 1024px) {
  .crd-styling {
    height: calc(100dvh - 8px * 6);
  }

  .crd-table-styling {
    height: calc(100dvh - 8px * 27);
  }

  .facts-view {
    margin: 0;
  }
}
</style>
