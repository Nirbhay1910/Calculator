# CALCULATOR
This project is made by me during my internship with Grapecity.
<br><br>
<ul>
<li>It is not a simple calculator. The structure of calculator library is very robust and scalable. </li>
<li>In this library user can add their own custom operaators by just inheriting one class "Binary or Unary" and can even make their own operations like ternary and all.</li>
<li>User can also change precedence of operator by just changing its value in JSON file.</li>
<li>Even in UI all operations are added dynamically i.e. If we add one more custom operator, it will be shown automatically in UI.</li>
<li>Apart from basic validations I also implemented validations like:
<ul>
<li>1. It will automatically add * operator in cases like 2(3), log(2)log(1) etc.</li>
<li>2. Use of '.' (decimal) operator eg user can't insert numbers like 2..., 2.3.4 etc.</li>
</ul>
</li>
</ul>
