import { gql } from "@apollo/client";

export const FILTER_FARM_TYPES = gql`
  mutation ($criterias: BaseSelectFilterModelInput) {
    selections: farmTypes(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const FILTER_FARMS = gql`
  mutation ($criterias: FarmSelectFilterModelInput) {
    selections: selectUserFarms(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const CREATE_FARM = gql`
  mutation ($criterias: CreateFarmModelInput!) {
    createFarm(criterias: $criterias) {
      id
    }
  }
`;

export const UPDATE_FARM = gql`
  mutation ($criterias: UpdateFarmModelInput!) {
    updateFarm(criterias: $criterias) {
      id
    }
  }
`;

export const DELETE_FARM = gql`
  mutation ($criterias: FarmIdFilterModelInput!) {
    deleteFarm(criterias: $criterias)
  }
`;
