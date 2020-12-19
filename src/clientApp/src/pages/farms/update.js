import React, { Fragment, useEffect } from "react";
import { GET_FARM_FOR_UPDATE } from "../../utils/GraphQLQueries/queries";
import {
  VALIDATE_IMAGE_URL,
  FILTER_FARM_TYPES,
  UPDATE_FARM,
} from "../../utils/GraphQLQueries/mutations";
import { useQuery, useMutation } from "@apollo/client";
import graphqlClient from "../../utils/GraphQLClient/graphqlClient";
import { withRouter } from "react-router-dom";
import { useStore } from "../../store/hook-store";
import { fileToBase64 } from "../../utils/Helper";
import Loading from "../../components/atoms/Loading";
import FarmEditor from "../../components/organisms/ProfileEditors/FarmEditor";
import ErrorBlock from "../../components/atoms/ErrorBlock";
import Breadcrumb from "../../components/organisms/Navigation/Breadcrumb";
import farmCreationModel from "../../models/farmCreationModel";

export default withRouter(function (props) {
  const { match } = props;
  const { params } = match;
  const { id } = params;
  const dispatch = useStore(false)[1];
  const [validateImageUrl] = useMutation(VALIDATE_IMAGE_URL);
  const [updateFarm] = useMutation(UPDATE_FARM, {
    client: graphqlClient,
  });
  const [farmTypes] = useMutation(FILTER_FARM_TYPES);

  const { loading, data, error, refetch, called } = useQuery(
    GET_FARM_FOR_UPDATE,
    {
      variables: {
        criterias: {
          id: parseFloat(id),
        },
      },
    }
  );

  const searchFarmTypes = async (inputValue) => {
    return await farmTypes({
      variables: {
        criterias: { query: inputValue },
      },
    })
      .then((response) => {
        var { data } = response;
        var { categories } = data;
        if (!categories) {
          return [];
        }
        return categories.map((cat) => {
          return {
            value: cat.id,
            label: cat.text,
          };
        });
      })
      .catch((error) => {
        console.log(error);
        return [];
      });
  };

  const convertImagefile = async (file) => {
    const url = await fileToBase64(file);
    return {
      url,
      fileName: file.name,
    };
  };

  const onImageValidate = async (value) => {
    return await validateImageUrl({
      variables: {
        criterias: {
          url: value,
        },
      },
    });
  };

  const onFarmPost = async (data) => {
    return await updateFarm({
      variables: {
        criterias: data,
      },
    }).then((response) => {
      return new Promise((resolve) => {
        const { data } = response;
        const { updateFarm: farm } = data;
        if (props.location.state && props.location.state.from) {
          const referrefUri = props.location.state.from;
          const farmUpdateUrl = `/farms/update/${farm.id}`;
          if (referrefUri !== farmUpdateUrl) {
            raiseFarmUpdatedNotify(farm);
            props.history.push(referrefUri);
            resolve({ farm });
            return;
          }
        }

        raiseFarmUpdatedNotify(farm);
        props.history.push(`/farms/${farm.id}`);
        resolve({ farm });
      });
    });
  };

  const raiseFarmUpdatedNotify = (farm) => {
    dispatch("FARM_UPDATE", {
      id: farm.id,
    });
  };

  const showValidationError = (title, message) => {
    dispatch("NOTIFY", {
      title,
      message,
      type: "error",
    });
  };

  useEffect(() => {
    if (!loading && called) {
      window.scrollTo(0, 0);
      refetch();
    }
  }, [refetch, called, loading]);

  if (loading || !data) {
    return <Loading>Loading...</Loading>;
  } else if (error) {
    return <ErrorBlock>Error!</ErrorBlock>;
  }

  const { farm } = data;

  const currentFarm = JSON.parse(JSON.stringify(farmCreationModel));
  for (const formIdentifier in currentFarm) {
    currentFarm[formIdentifier].value = farm[formIdentifier];
    if (farm[formIdentifier]) {
      currentFarm[formIdentifier].isValid = true;
    }
  }

  const breadcrumbs = [
    {
      title: "Farms",
      url: "/farms/",
    },
    {
      title: farm.name,
      url: `/Farms/${farm.id}`,
    },
    {
      isActived: true,
      title: "Update",
    },
  ];

  return (
    <Fragment>
      <Breadcrumb list={breadcrumbs} />
      <FarmEditor
        height={350}
        convertImageCallback={convertImagefile}
        onImageValidate={onImageValidate}
        filterCategories={searchFarmTypes}
        onFarmPost={onFarmPost}
        showValidationError={showValidationError}
        currentFarm={currentFarm}
      />
    </Fragment>
  );
});
