#include "stdafx.h"
#include <mpi.h>
#include <list>
#include <random>
#include <iostream>
#include <string>
#include <cmath>
using namespace std;


// Размерность массива
const int n = 5;


//находим максимальный отрицательный
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
	int res = pow(2.0,rv);
	return res;
}

void max_pow(int* invec, int* outvec, int* len, MPI_Datatype *dtp)
{
	int cnt = *len;
	
	for(int i = 0; i< cnt; i++)
	{
		int invec_max = calc_pow(invec[i]);
		int inout_max = calc_pow(outvec[i]);
		outvec[i] = invec_max > inout_max ? invec_max : inout_max;
	}
}

double log(double a, double b)
{
    return log(b) / log(a);
}
 

int _tmain(int argc, char* argv[])
{
	// Инициализация MPI
	MPI_Init(&argc,&argv);

	int processCount = 0;
	MPI_Comm_size(MPI_COMM_WORLD,&processCount);

	if (processCount>1)
	{
		int *arr = new int[n];
		srand((unsigned int)arr);

		int rank = 0;
		MPI_Comm_rank(MPI_COMM_WORLD, &rank);

		string str = "Process " + to_string((_Longlong)rank) + " generated elements:\t";

		// Заполнение массива и его вывод на экран.
		for (int i=0; i<n; i++)
		{
			arr[i]=(rand()*(rank+1)-rand()*2)%100;
			str+= to_string((_Longlong)arr[i]) + '\t';
		}
		printf("%s\n",str.c_str());

		//инициализируем нашу операцию для редукции
		MPI_Op operation;
		MPI_Op_create((MPI_User_function*)max_pow,1,&operation);
		int *max = new int[n];
		MPI_Reduce(arr,max,n,MPI_INT,operation,0,MPI_COMM_WORLD);

		if (rank==0)
		{
			string resultString = "Result: \t\t\t";
			for ( int j = 0; j < n; j++)
				resultString += to_string((_Longlong)log(2,max[j])) + '\t';
			printf("%s\n",resultString.c_str());
		}
	}

	MPI_Finalize();
	return 0;
}