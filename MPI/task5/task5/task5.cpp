#include "stdafx.h"
#include <mpi.h>
#include <random>
#include <time.h>
#include <iostream>
#include <string>
#pragma warning(disable : 4996)

using namespace std;

int sum(int *a,int *b, int len)
{
	int sum = 0;
	for (int i =0; i<len; i++)
		sum+=a[i]*b[i];
	return sum;
}


int _tmain(int argc, char* argv[])
{
	const int DIMENSION = 1;

	int i,j,size,rank,rank_pred,rank_next,max,current;
	int dims[DIMENSION],periods[DIMENSION],new_coords[DIMENSION];
	MPI_Status status;
	MPI_Comm new_comm;

	MPI_Init(&argc,&argv);
	MPI_Comm_rank(MPI_COMM_WORLD,&rank);
	MPI_Comm_size(MPI_COMM_WORLD,&size);

	int* A = new int[size];
	int* B = new int[size];
	for(i=0; i<DIMENSION; i++)
	{
		dims[i]=0;
		periods[i]=1;
	}
	
	MPI_Dims_create(size, DIMENSION, dims); 
	MPI_Cart_create (MPI_COMM_WORLD, DIMENSION, dims, periods, 0, &new_comm);
	MPI_Cart_coords (new_comm, rank, DIMENSION, new_coords);
	MPI_Cart_shift(new_comm, 0, -1, &rank_pred, &rank_next);

	char str[300]="A[";
	char buf[5];
	itoa(rank,buf,10);
	strncat(str,buf,strlen(buf));
	strncat(str,"] = [",5);
	srand(time(NULL)+rank);	
	for (i=0;i<size;i++)
	{
		A[i]=rand()%100;
		itoa(A[i],buf,10);
		strncat(str,buf,strlen(buf));
		strncat(str,", ",2);
	}

	strncat(str,"]\nB[",5);
	itoa(rank,buf,10);
	strncat(str,buf,strlen(buf));
	strncat(str,"] = [",5);
	for (i=0;i<size;i++)
	{
		B[i]=rand()%100;
		itoa(B[i],buf,10);
		strncat(str,buf,strlen(buf));
		strncat(str,"  ",2);
	}

	strncat(str,"]\nResults:\n   sum(A[",22);
	itoa(rank,buf,10);
	strncat(str,buf,strlen(buf));
	strncat(str,",k]*B[k,0]) = ",14);
	current=sum(A,B,size);
	max=current;				
	itoa(current,buf,10);
	strncat(str,buf,strlen(buf));
	strncat(str,"\n",2);
	for (j=1;j<size;j++) 
	{
		MPI_Sendrecv_replace(B,size,MPI_INT,rank_next,2,rank_pred,2,new_comm,&status); 
		current=sum(A,B,size); 
		if (current>max) 
			max=current;
		strncat(str,"   sum(A[",9);
		itoa(rank,buf,10);
		strncat(str,buf,strlen(buf));
		strncat(str,",k]*B[k,",8);
		itoa(j,buf,10);
		strncat(str,buf,strlen(buf));
		strncat(str,"]) = ",5);
		itoa(current,buf,10);
		strncat(str,buf,strlen(buf));
		strncat(str,"\n",2);				   
	}
	
	strncat(str," Maximum = ",11);
	itoa(max,buf,10);
	strncat(str,buf,strlen(buf));
	strncat(str,"\n",2);
	printf("%s\n",str);

	MPI_Comm_free(&new_comm) ;
	free(A);
	free(B);

	MPI_Finalize();
	return 0;
}