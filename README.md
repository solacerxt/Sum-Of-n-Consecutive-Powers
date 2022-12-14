# Sum-Of-n-Consecutive-Powers

Given integers $n \geq 1$, $p \geq 0$. Suppose we have found a sequence of rational numbers $\langle a_1, \ldots, a_{p+1} \rangle$ such that

```math
\displaystyle\sum_{k=1}^{n} k^{p} = \displaystyle\sum_{i=1}^{p+1} a_{i}n^{i}
```
Then we can calculate the sum of $n$ consecutive powers in $O(p)$.

## Algorithm finding a[1..p+1]

```csharp
var a = new Fraction64[p + 2];
var binom = new long[p + 2, p + 2];

for (var i = 0; i < p + 2; ++i)
{
    binom[i, 0] = binom[i, i] = 1;
    
    for (var j = 1; j < i; ++j)
        binom[i, j] = binom[i - 1, j] + binom[i - 1, j - 1];
}

a[p + 1] = (1, p + 1); // (numerator, denominator)

for (var i = p; i > 0; --i)
{
    a[i] = (0, 1);
    var sign = 1;
    for (var m = i + 1; m <= p + 1; ++m)
    {
        a[i] += sign * binom[m, i - 1] * a[m]; // overloaded operators * and + with division by GCD
        sign *= -1;
    }

    a[i] /= i;
}

//Answer: a[1..p+1]

```
Complexity: Time – $O(p^2)$, Memory – $O(p^2)$.

## Proof (in Russian)

Пусть у нас есть последовательность рациональных чисел $a = \langle a_0, \ldots, a_{p+1} \rangle$ такая, что значение k-го слагаемого можно представить как разность:

```math 
k^p = f_p(k) - f_p(k - 1),  \text{ (*)}
``` 
```math 
f_p(k) = \displaystyle\sum_{i=0}^{p+1} a_{i}k^{i}.
``` 
Тогда 
```math
\displaystyle\sum_{k=1}^{n} k^{p} = \displaystyle\sum_{k=1}^{n} (f_p(k) - f_p(k - 1)) = f_p(n) - f_p(0)
= \displaystyle\sum_{i=0}^{p+1} a_{i}n^{i} - a_0
\Leftrightarrow
```
```math
\Leftrightarrow
\displaystyle\sum_{k=1}^{n} k^{p} = \displaystyle\sum_{i=1}^{p+1} a_{i}n^{i}. \text{ (**)}
```
\
\
В силу $\text{(*)}$ получаем равенство
```math
k^p = \displaystyle\sum_{i=0}^{p+1} a_{i}k^{i} - \displaystyle\sum_{i=0}^{p+1} a_{i}(k - 1)^{i}
```

Раскроем множитель $(k-1)^i$ во второй сумме по формуле бинома Ньютона
```math
k^p = \displaystyle\sum_{i=0}^{p+1} a_{i}k^{i} - \displaystyle\sum_{i=0}^{p+1} a_{i} \displaystyle\sum_{j=0}^{i} \binom{i}{j} k^{i-j} (-1)^j
```
Представим $k^p$ в виде $\displaystyle\sum_{i=0}^{p+1} b_{i}k^{i}$, где $b_p = 1$, а остальные $b_i = 0$ $(i=0..p+1,$ $i \neq p)$ и найдём для фиксированного $u \in [0..p+1]$ выражение через сумму для коэффициента $b_u$ перед $k^u$:
* В первой сумме $k^u$ встречается один раз, при $i=u$. Значит $b_u = a_u + \ldots$ .
* Оставшиеся слагаемые берутся из раскрытия второй суммы - коэффициенты перед $k^{i-j}$. Чтобы их определить, нужно найти все такие $i$ и $j$ , что $i - j = u$. Т.к. $0 \leq j \leq i \leq p + 1$, то выражая $j$ через $i$, получаем все решения: $i = m$, $j = m - u$ для $m$ из $[u..p+1]$
<!-- end of the list -->
Таким образом
$$b_u = a_u - \displaystyle\sum_{m=u}^{p+1} a_m \binom{m}{m-u} (-1)^{m-u}, $$
что равносильно
```math
b_u =
  \begin{cases}
    0,                                                             & \quad \text{если } u=p+1\\
    -\displaystyle\sum_{m=u+1}^{p+1} a_m \binom{m}{u} (-1)^{m-u},& \quad \text{если } 0 \leq u < p+1
  \end{cases}
```
Т.к. выражение коэффициента $b_u$ состоит из всех $a_i : u+1 \leq i \leq p + 1$, то мы можем вычислить все $a_i$ для $1 \leq i \leq p+1$ (вспомним, что $a_0$ в формуле $\text{(**)}$ нам не нужен):
* $a_{p+1}$ :<br>
```math
\begin{cases}
  b_p = -a_{p+1} \binom{p+1}{p} (-1)^1,\\
  b_p = 1
\end{cases}
\Rightarrow
a_{p+1} (p+1) = 1
\Leftrightarrow
a_{p+1} = \frac{1}{p+1}.
```
* $a_{i} : 1 \leq i \leq p$ :<br>
```math
\begin{cases}
  b_{i-1} = -\displaystyle\sum_{m=i}^{p+1} a_m \binom{m}{i-1} (-1)^{m-i+1},\\
  b_{i-1} = 0
\end{cases}
\Rightarrow
\displaystyle\sum_{m=i}^{p+1} a_m \binom{m}{i-1} (-1)^{m-i+1} = 0
\Leftrightarrow
```
```math
\Leftrightarrow
a_i \binom{i}{i-1} (-1)^{i-i+1} + \displaystyle\sum_{m=i+1}^{p+1} a_m \binom{m}{i-1} (-1)^{m-i+1} = 0
\Leftrightarrow
```
```math
\Leftrightarrow
a_i = \frac{1}{i} \displaystyle\sum_{m=i+1}^{p+1} a_m \binom{m}{i-1} (-1)^{m-i+1}.
```
<br> Таким образом, мы можем последовательно вычислять $a_i$ в цикле начиная c $i = p$ до $1$, сперва вычислив $a_{p+1}$. 

------------------------------------
Оценим вычислительную сложность.<br>
* Память – $O(p^2)$ :<br> 
  Нужно хранить $a_0, \ldots, a_{p+1}$ – это требует $O(p)$ памяти. Также нам нужно хранить биномиальные коэффициенты, чтобы не вычислять их каждый раз заново – $O(p^2)$.

* Время – $O(p^2)$ : <br> 
  При вычислении каждого $a_i$ для $i \in [1..p]$ происходит суммирование $p - i + 1$ чисел – суммарно $O(1 + 2 + \ldots + p) = O(p^2)$, а $a_{p+1}$ вычисляется за $O(1)$. Предварительное вычисление биномиальных коэффициентов требует $O(p^2)$ времени. 
  
------------------------------------

Т.к. в компьютере дробные числа теряют точность, $a_i$ лучше хранить в виде пары целых чисел - (числитель, знаменатель), где знаменатель $> 0$. Но это не решит проблему вычисления суммы по формуле, которую мы даём конечному пользователю, поскольку всё равно придётся делить на знаменатель. Можно, конечно, до последнего оставлять ответ в виде дроби, но в таком случае может возникнуть переполнение даже на не очень больших входных данных. Во избежание этого при вычислении суммы после каждого сложения нужно будет делить на наибольший общий делитель. Часто будет проще и эффективнее, если ответ выдавать в виде такой последовательности целых чисел $a’$, что: 
```math
\frac{1}{a’_0} \displaystyle\sum_{i=1}^{p+1} a’_{i}n^{i} = \displaystyle\sum_{i=1}^{p+1} a_{i}n^{i}
```
Таким образом, нужно лишь просуммировать все $a’_i n^i$ и потом поделить на $a'_0$. Т.к. в итоге должно получиться целое число, то целочисленное деление здесь сработает корректно.

<br> Считая, что $a_i$ хранится в виде дроби $\frac{x_i}{y_i}$ (желательно привести её к несократимой), получить $a’$ по $a$ можно следующим образом:
```math
a’_0 = \text{lcm}(y_1,y_2, \ldots, y_{p+1}),
```
```math
a’_i = a’_0\frac{x_i}{y_i}
```
Временная сложность данного преобразования – $O(p)$.
