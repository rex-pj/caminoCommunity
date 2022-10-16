import { gql } from "@apollo/client";

export const GET_USER_FARMS = gql`
  query ($criterias: FarmFilterModelInput) {
    userFarms(criterias: $criterias) {
      totalPage
      totalResult
      filter {
        page
        pageSize
        search
      }
      collections {
        id
        description
        name
        createdBy
        createdDate
        updatedDate
        createdByIdentityId
        createdByPhotoId
        address
        pictures {
          pictureId
        }
      }
    }
  }
`;

export const SELECT_USER_FARMS = gql`
  query ($criterias: FarmFilterModelInput) {
    userFarms(criterias: $criterias) {
      collections {
        id
        name
      }
    }
  }
`;

export const GET_FARMS = gql`
  query ($criterias: FarmFilterModelInput) {
    farms(criterias: $criterias) {
      totalPage
      totalResult
      filter {
        page
        pageSize
        search
      }
      collections {
        id
        description
        name
        createdBy
        createdDate
        updatedDate
        createdByIdentityId
        createdByPhotoId
        address
        pictures {
          pictureId
        }
      }
    }
  }
`;

export const GET_FARM = gql`
  query ($criterias: FarmIdFilterModelInput) {
    farm(criterias: $criterias) {
      id
      description
      name
      createdBy
      createdDate
      updatedDate
      address
      createdByIdentityId
      createdByPhotoId
      pictures {
        pictureId
      }
    }
  }
`;

export const GET_FARM_FOR_UPDATE = gql`
  query ($criterias: FarmIdFilterModelInput) {
    farm(criterias: $criterias) {
      id
      description
      name
      createdBy
      createdDate
      updatedDate
      address
      createdByIdentityId
      createdByPhotoId
      farmTypeName
      farmTypeId
      pictures {
        pictureId
      }
    }
  }
`;
