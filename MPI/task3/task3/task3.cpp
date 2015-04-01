// task1.cpp : Defines the entry point for the console application.
//������������ ������� ������, �� ������������� ������������ ��������

#include "stdafx.h"
#include <mpi.h>
#include <random>
#include <time.h>
#include <iostream>
#include <string>

using namespace std;
void max_pow(int* , int* , int* , MPI_Datatype *);

int calc_pow(int max)
{
	if (max<=0)
		return 0;
	int rv = 0;
	max = (max >> 1);
	for(; max>0 ; )
	{
		rv++;
		max = (max >> 1);
	}
	return rv;
}

void max_pow(int* invec, int* outvec, int* len, MPI_Datatype *dtp)
{
	int max = invec[0];
	for(int i = 0; i<*len; i++)
		if (max < invec[i])
			max = invec[i];
	int maxp = calc_pow(max);
	for(int i = 0; i<*len; i++)
		outvec[i] = maxp;
}


int _tmain(int argc, char* argv[])
{
	const int n = 5;
	int* data = new int[n];
	int* result = new int[n];
	int rank;
	int numproc;
	int tag = 0;
	char str[100]="Process: ";
	char buf[5];
	MPI_Op op;
	MPI_Status status;
	MPI_Init(&argc, &argv);
	MPI_Comm_size(MPI_COMM_WORLD, &numproc);
	MPI_Comm_rank(MPI_COMM_WORLD, &rank);
	itoa(rank,buf,10);
	strncat(str,buf,strlen(buf));
	strncat(str," array: ",8);
	srand(time(NULL)+rank);
	
	for(int i = 0; i<n; i++)
	{
		data[i] = (rand()*(rank+1)-rand()*2)%129; 
		itoa(data[i],buf,10);
		strncat(str,buf,strlen(buf));
		strncat(str,", ",2);
		result[i]=0;
	}
	cout << str << endl;;
	MPI_Op_create((MPI_User_function *)max_pow, 1, &op);
	MPI_Reduce(&data,&result,n,MPI_INT,op,0,MPI_COMM_WORLD);
	MPI_Op_free(&op);

	if (rank == 0)
	{
		cout << "powers:" << endl;
		for(int i = 0; i<n; i++)
			cout << result[i] << ' ';
	}

	MPI_Finalize();
	system("pause");
	return 0;
}
