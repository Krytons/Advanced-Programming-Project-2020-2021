list.of.packages <- c("taskscheduleR")
noninst.packages <- list.of.packages[!(list.of.packages %in% installed.packages()[,"Package"])]
if(length(noninst.packages)) install.packages(noninst.packages)

lapply(list.of.packages, require, character.only = TRUE)

#################################################