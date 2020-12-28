import { gql } from "@apollo/client";

export const FILTER_FARM_TYPES = gql`
  mutation($criterias: SelectFilterModelInput) {
    categories: farmTypes(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const FILTER_FARMS = gql`
  mutation($criterias: SelectFilterModelInput) {
    userFarms: selectUserFarms(criterias: $criterias) {
      id
      isSelected
      text
    }
  }
`;

export const CREATE_FARM = gql`
  mutation($criterias: FarmModelInput!) {
    createFarm(criterias: $criterias) {
      id
    }
  }
`;

export const UPDATE_FARM = gql`
  mutation($criterias: FarmModelInput!) {
    updateFarm(criterias: $criterias) {
      id
    }
  }
`;

export const DELETE_FARM = gql`
  mutation($criterias: FarmFilterModelInput!) {
    deleteFarm(criterias: $criterias)
  }
`;
